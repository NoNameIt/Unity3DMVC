﻿using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  ViewBase.cs
//  Author: Lu Zexi
//  2014-07-05



/// <summary>
/// The View Base
/// </summary>
public class UIViewBase : MonoBehaviour
{
	void Awake()
	{
		Type t = GetType();
		FieldInfo[] fis = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
		foreach (FieldInfo f in fis)
		{
			Type fieldType = f.FieldType;
			object obj = f.GetValue(this);
			if( obj is List<GameObject> )
			{
				List<GameObject> lst = new List<GameObject>();
				for( int i = 1 ;;i++)
				{
					Transform tra = FIND_CHILD(this.transform , f.Name+i);
					if(tra== null)
					{
						f.SetValue(this,lst);
						break;
					}
					lst.Add(tra.gameObject);
				}
			}
			else if( obj is GameObject || fieldType.IsSubclassOf(typeof(MonoBehaviour)))
			{
				Transform tra = FIND_CHILD(this.transform , f.Name);
				if(tra != null)
				{
					if( fieldType == typeof(GameObject) )
					{
						var com = tra.gameObject;
						f.SetValue(this,com);
					}
					else
					{
						var com = tra.GetComponent(fieldType);
						f.SetValue(this,com);
					}
				}
			}
		}
	}


	protected Transform FIND_CHILD( Transform parent , string childName )
	{
		foreach( Transform item in parent )
		{
			if(item.name == childName)
			{
				return item;
			}
			else
			{
				Transform res = FIND_CHILD(item , childName);
				if(res != null )
					return res;
			}
		}
		return null;
	}
}
