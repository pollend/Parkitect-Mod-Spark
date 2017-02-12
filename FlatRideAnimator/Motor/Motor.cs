using UnityEngine;
using System;
using UnityEditor;

[Serializable]
public class Motor : ScriptableObject
{
	public bool showSettings;
	public string Identifier = "";
	public Color ColorIdentifier;
	public virtual string EventName { set; get; }
	public void Awake()
	{
		ColorIdentifier = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
	}
	public virtual void DrawGUI(Transform root)
	{
		ColorIdentifier = EditorGUILayout.ColorField("Color ", ColorIdentifier);
	}
	public virtual void Enter(Transform root)
	{

	}
	public virtual void Reset(Transform root)
	{

	}
}