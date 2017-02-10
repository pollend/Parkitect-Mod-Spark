using System;
using UnityEngine;
using UnityEditor;

public class PathTypeDecorator : Decorator
{
	public enum PathType { Normal, Queue, Employee }
	public PathType pathType;
	public Texture2D pathTexture;

	public PathTypeDecorator ()
	{
	}

	#if UNITY_EDITOR
    public override void RenderInspectorGUI (ParkitectObj parkitectObj)
	{
		this.pathType = (PathTypeDecorator.PathType)EditorGUILayout.EnumPopup("Type", this.pathType);


		this.pathTexture = (Texture2D)EditorGUILayout.ObjectField("Texture",this.pathTexture, typeof(Texture2D), true);
		if(GUILayout.Button("Create") && this.pathTexture)
		{
			this.pathTexture.alphaIsTransparency = true;
			this.pathTexture.wrapMode = TextureWrapMode.Repeat;
			this.pathTexture.filterMode = FilterMode.Point;

			AssetDatabase.DeleteAsset("Assets/Materials/Paths/" + parkitectObj.name + ".mat");
			parkitectObj.GameObjectRef.gameObject.AddComponent<MeshRenderer>();
			MeshRenderer MR = parkitectObj.GameObjectRef.GetComponent<MeshRenderer>();

			//Check Folder for the mat
			if (!AssetDatabase.IsValidFolder("Assets/Materials"))
				AssetDatabase.CreateFolder("Assets", "Materials");
			if (!AssetDatabase.IsValidFolder("Assets/Materials/Paths"))
				AssetDatabase.CreateFolder("Assets/Materials", "Paths");
			Material material = new Material(Shader.Find("Transparent/Diffuse"));
			material.mainTexture = this.pathTexture;
			AssetDatabase.CreateAsset(material, "Assets/Materials/Paths/" + parkitectObj.name + ".mat");
			MR.material = material;

			parkitectObj.GameObjectRef.gameObject.AddComponent<MeshFilter>();
			MeshFilter MF = parkitectObj.GameObjectRef.GetComponent<MeshFilter>();
			GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Quad);
			MF.mesh = GO.GetComponent<MeshFilter>().sharedMesh;
			MonoBehaviour.DestroyImmediate(GO);
			parkitectObj.GameObjectRef.gameObject.transform.eulerAngles = new Vector3(90,0,0);
		}
        base.RenderInspectorGUI (parkitectObj);
	}
	#endif
}


