using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BaseDecorator : Decorator
{
	public BaseDecorator ()
	{
	}


	[SerializeField]
	public string InGameName;

	[SerializeField]
	public float price;

	#if UNITY_EDITOR
	public override void Render (ParkitectObj parkitectObj)
	{
		this.InGameName = EditorGUILayout.TextField("In Game name: ", this.InGameName);
		this.price = EditorGUILayout.FloatField("Price: ", this.price);
		base.Render (parkitectObj);
	}
	#endif

}


