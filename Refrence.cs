using System;
using UnityEngine;

public class Refrence : MonoBehaviour
{
	[SerializeField]
	private string refrence;

	public string getRefrence(){
		return refrence;
	}

	public String Key{
		get{
			if(refrence == null || refrence == "")
				this.refrence = System.Guid.NewGuid ().ToString ();
			return refrence;
		}
	}

	public static Transform findTransformByKey(Transform root, String key)
	{

		for(int i = 0; i < root.childCount;i++ ) {
			var temp  = root.GetChild(i);
			var refrence = temp.gameObject.GetComponent<Refrence> ();

			if (refrence != null) {
				if (refrence.Key == key)
					return temp;
			}
			Transform result = findTransformByKey (temp, key);
			if (result != null)
				return result;
		}

		var refr = root.gameObject.GetComponent<Refrence> ();
		if (refr != null) {
			if (refr.Key == key) {
				return root;
			}
		}
		return null;
	}

}

