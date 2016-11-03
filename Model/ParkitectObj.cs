using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Security.Cryptography;

[Serializable]
public class ParkitectObj : ScriptableObject
{
	//Basic
	[SerializeField]
	private List<Decorator> decorators;
	[SerializeField]
	private string gameObjectRef;
	[SerializeField]
	public string name;
	public float XSize;

	public GameObject gameObject{ 
		get 
		{ 
			return GameObject.Find (gameObjectRef).transform.parent.gameObject;
		}
		set{ 
			Transform gameRef =  value.gameObject.transform.Find ("pkref");
			if (gameRef == null) {
				gameRef = new GameObject (System.Guid.NewGuid().ToString()).transform;
				gameRef.transform.parent = value.transform;

			}
			gameObjectRef = gameRef.name;
		}
	}

	public ParkitectObj()
	{
		if (decorators == null)
			decorators = new List<Decorator> ();
	}

	public void Load(ParkitectObj parkitectObj)
	{
		this.decorators = parkitectObj.decorators;
		this.name = parkitectObj.name;
		this.gameObjectRef = parkitectObj.gameObject.name;
		this.XSize = parkitectObj.XSize;

	}


	public virtual Type[] SupportedDecorators()
	{
		return new Type[]{ };
	}

	public ParkitectObj (Decorator[] decorators)
	{
		this.decorators = new List<Decorator> ();
		for (int x = 0; x < decorators.Length; x++) {
			this.decorators.Add (decorators [x]);
		}
	}

	public Decorator GetDecorator(Type t)
	{
		
		for (int x = 0; x < decorators.Count; x++)
		{
			if (decorators [x] != null) {
				if (decorators [x].GetType ().Equals (t)) {
					return decorators [x];
				}
			}
		}

		Decorator dec = (Decorator)ScriptableObject.CreateInstance (t);

		AssetDatabase.AddObjectToAsset (dec,this);
		EditorUtility.SetDirty(dec);
		EditorApplication.SaveAssets();
		AssetDatabase.SaveAssets ();

		if(!EditorSceneManager.GetActiveScene().isDirty)
			EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene());
		


		decorators.Add (dec);
		return dec;
		
	}
		
	public virtual void Load()
	{
	}

}

