using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
[Serializable]
public class MultipleRotations : Motor {
    [SerializeField]
	public RefrencedTransform mainAxis = new RefrencedTransform();
    [SerializeField]
	public List<RefrencedTransform> Axiss = new List<RefrencedTransform>();
    
	public override void DrawGUI(Transform root)
    {

        Identifier = EditorGUILayout.TextField("Name ", Identifier);
		mainAxis.SetSceneTransform((Transform)EditorGUILayout.ObjectField("MainAxis", mainAxis.FindSceneRefrence(root), typeof(Transform), true));
        Transform Axis = (Transform)EditorGUILayout.ObjectField("Add axis", null, typeof(Transform), true);
        if(Axis)
        {
			var refrenceTransform =  new RefrencedTransform ();
			refrenceTransform.SetSceneTransform (Axis);
			Axiss.Add(refrenceTransform);
        }
        if (Selection.objects.Length > 0)
        {
            if (GUILayout.Button("Add selection"))
            {
                foreach (GameObject GObj in Selection.objects)
                {
					var refrenceTransform =  new RefrencedTransform ();
					refrenceTransform.SetSceneTransform (GObj.transform);
					Axiss.Add(refrenceTransform);
                }

            }
        }
		foreach (RefrencedTransform T in Axiss)
        {
			if(GUILayout.Button(T.FindSceneRefrence(root).gameObject.name, "ShurikenModuleTitle"))
            {
                if (Event.current.button == 1)
                {
                    Axiss.Remove(T);
                    return;
                }
                else
                {
					Selection.objects = new GameObject[] { T.FindSceneRefrence(root).gameObject};
					EditorGUIUtility.PingObject(T.FindSceneRefrence(root).gameObject);
                }
            }
        }
		base.DrawGUI(root);
    }
	public override void Reset(Transform root)
    {
		Transform transform = mainAxis.FindSceneRefrence (root);
		if (transform)
        {
			foreach (RefrencedTransform T in Axiss)
            {
				T.FindSceneRefrence(root).localRotation = transform.localRotation;
            }
        }
    }
	public void tick(float dt,Transform root)
    {
		Transform transform = mainAxis.FindSceneRefrence (root);
		if (transform)
        {
			foreach (RefrencedTransform T in Axiss)
            {
				T.FindSceneRefrence(root).localRotation = transform.localRotation;
            }
        }
    }
}
