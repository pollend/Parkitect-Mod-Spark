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
	private GameObject prefab;
	[SerializeField]
	public string key;

	[SerializeField]
	public string name;
	public float XSize;

	[NonSerialized]
	public GameObject sceneRef; 

	public string getKey{get{return key;}}

	public GameObject Prefab{ get {return prefab;} }

	public GameObject GameObjectRef{
		get{
			GameObject[] rootSceneNodes =  EditorSceneManager.GetActiveScene ().GetRootGameObjects ();
			for (int x = 0; x < rootSceneNodes.Length; x++) {
				var gameObject = rootSceneNodes [x].transform.Find (this.key);
				if (gameObject != null)
					return gameObject.parent.gameObject;
			}
			return null;
		}
	}

	public virtual GameObject SetGameObject(GameObject g)
	{
		Transform gameRef =  null;

		for(int i = 0; i < g.transform.childCount; i++)
		{
			if (g.transform.GetChild (i).name.StartsWith ("pkref-")) {
				gameRef = g.transform.GetChild (i);
				break;
			}
		}

		if (gameRef == null) {
			gameRef = new GameObject ("pkref-" + System.Guid.NewGuid().ToString()).transform;
			gameRef.transform.parent = g.transform;

		}
		this.key = gameRef.name;

		var path = "Assets/Resources/" + g.name + ".prefab";
		GameObject prefab =  PrefabUtility.CreatePrefab (path, g);
		PrefabUtility.ConnectGameObjectToPrefab (g, prefab);
		name = prefab.name;
		this.prefab = prefab;
		return prefab;
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
		this.prefab = parkitectObj.prefab;
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
		AssetDatabase.SaveAssets();
		if(!EditorSceneManager.GetActiveScene().isDirty)
			EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene());
		

		decorators.Add (dec);
		return dec;
		
	}


		
	public virtual void Load()
	{
	}

    public void CleanUp()
    {
        for (int x = 0; x < decorators.Count; x++) {
			decorators [x].CleanUp ();
            UnityEngine.Object.DestroyImmediate (decorators[x], true);
        }
    }
   

}

