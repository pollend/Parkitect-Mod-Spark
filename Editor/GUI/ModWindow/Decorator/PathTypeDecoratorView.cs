using System;
using UnityEditor;
using UnityEngine;

public class PathTypeDecoratorView : IDecoratorView
{
	public PathTypeDecoratorView ()
	{
	}

	public void Render (Decorator decorator,ParkitectObj parkitectObj)
	{
		PathTypeDecorator pathTypeDecorator = (PathTypeDecorator)decorator;
		pathTypeDecorator.pathType = (PathTypeDecorator.PathType)EditorGUILayout.EnumPopup("Type", pathTypeDecorator.pathType);


		pathTypeDecorator.pathTexture = (Texture2D)EditorGUILayout.ObjectField("Texture",pathTypeDecorator.pathTexture, typeof(Texture2D), true);
		if(GUILayout.Button("Create") && pathTypeDecorator.pathTexture)
		{
			pathTypeDecorator.pathTexture.alphaIsTransparency = true;
			pathTypeDecorator.pathTexture.wrapMode = TextureWrapMode.Repeat;
			pathTypeDecorator.pathTexture.filterMode = FilterMode.Point;

			AssetDatabase.DeleteAsset("Assets/Materials/Paths/" + parkitectObj.name + ".mat");
			parkitectObj.gameObject.AddComponent<MeshRenderer>();
			MeshRenderer MR = parkitectObj.gameObject.GetComponent<MeshRenderer>();

			//Check Folder for the mat
			if (!AssetDatabase.IsValidFolder("Assets/Materials"))
				AssetDatabase.CreateFolder("Assets", "Materials");
			if (!AssetDatabase.IsValidFolder("Assets/Materials/Paths"))
				AssetDatabase.CreateFolder("Assets/Materials", "Paths");
			Material material = new Material(Shader.Find("Transparent/Diffuse"));
			material.mainTexture = pathTypeDecorator.pathTexture;
			AssetDatabase.CreateAsset(material, "Assets/Materials/Paths/" + parkitectObj.name + ".mat");
			MR.material = material;

			parkitectObj.gameObject.AddComponent<MeshFilter>();
			MeshFilter MF = parkitectObj.gameObject.GetComponent<MeshFilter>();
			GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Quad);
			MF.mesh = GO.GetComponent<MeshFilter>().sharedMesh;
			MonoBehaviour.DestroyImmediate(GO);
			parkitectObj.gameObject.transform.eulerAngles = new Vector3(90,0,0);
		}
	}

}


