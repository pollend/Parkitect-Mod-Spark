using System;
using UnityEngine;
using UnityEditor;

public class ModConfigurationView
{


	public void Render()
	{
		GUILayout.Label("Mod setup", "PreToolbar");
		GUILayout.Label("Mod Name:", EditorStyles.boldLabel);
		ModPayload.Instance.name = EditorGUILayout.TextField(ModPayload.Instance.name);
		GUILayout.Label("Mod Description:", EditorStyles.boldLabel);
		ModPayload.Instance.description = EditorGUILayout.TextArea(ModPayload.Instance.description);
		GUILayout.Space(10);
	}
}


