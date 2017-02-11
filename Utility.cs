using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public static class Utility
{
	public static void recursiveFindTransformsStartingWith(string name, Transform parentTransform, List<Transform> transforms)
	{
		Transform[] componentsInChildren = parentTransform.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Transform transform = componentsInChildren[i];
			if (transform.name.StartsWith(name))
			{
				transforms.Add(transform);
			}
		}
	}

	public static void findAllChildrenWithName(Transform transform,String name,List<GameObject> collection)
	{
		for(int i = 0; i < transform.childCount;i++ ) {
			var temp  = transform.GetChild(i);
			if (temp.name == name) {
				collection.Add (temp.gameObject);
			}
			findAllChildrenWithName (temp, name, collection);
		}
	}


}


