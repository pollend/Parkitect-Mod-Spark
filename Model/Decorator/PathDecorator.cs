using System;
using UnityEngine;
using UnityEditor;

public class  PathDecorator : Decorator
{
	public enum PathType { Normal, Queue, Employee }
	public PathType pathType;
	public Texture2D PathTexture;

	public PathDecorator ()
	{
	}
	#if UNITY_EDITOR
	public override void RenderInspectorGUI (ParkitectObj parkitectObj)
	{
		this.PathTexture = (Texture2D)EditorGUILayout.ObjectField("Texture",this.PathTexture, typeof(Texture2D), true);
		if(GUILayout.Button("Create") && this.PathTexture)
		{
			this.PathTexture.alphaIsTransparency = true;
			this.PathTexture.wrapMode = TextureWrapMode.Repeat;
			this.PathTexture.filterMode = FilterMode.Point;

			AssetDatabase.DeleteAsset("Assets/Materials/Paths/" + parkitectObj.getKey + ".mat");
			parkitectObj.Prefab.AddComponent<MeshRenderer>();
			MeshRenderer MR = parkitectObj.Prefab.GetComponent<MeshRenderer>();

			//Check Folder for the mat
			if (!AssetDatabase.IsValidFolder("Assets/Materials"))
				AssetDatabase.CreateFolder("Assets", "Materials");
			if (!AssetDatabase.IsValidFolder("Assets/Materials/Paths"))
				AssetDatabase.CreateFolder("Assets/Materials", "Paths");
			Material material = new Material(Shader.Find("Transparent/Diffuse"));
			material.mainTexture = this.PathTexture;
			AssetDatabase.CreateAsset(material, "Assets/Materials/Paths/" + parkitectObj.getKey + ".mat");
			MR.material = material;

			parkitectObj.Prefab.AddComponent<MeshFilter>();
			MeshFilter MF = parkitectObj.Prefab.GetComponent<MeshFilter>();
			GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Quad);
			MF.mesh = GO.GetComponent<MeshFilter>().sharedMesh;
			DestroyImmediate(GO);
			parkitectObj.Prefab.transform.eulerAngles = new Vector3(90,0,0);
		}

		base.RenderInspectorGUI (parkitectObj);
	}
	#endif
}


