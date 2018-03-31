using System;
using UnityEngine;
using UnityEditor;

public class ModConfigurationView
{


	public void Render()
	{
		GUILayout.Label("Mod setup", "PreToolbar");
		GUILayout.Label("Mod Name:", EditorStyles.boldLabel);
		ModPayload.Instance.ModName = EditorGUILayout.TextField(ModPayload.Instance.ModName);
		GUILayout.Label("Mod Description:", EditorStyles.boldLabel);
		ModPayload.Instance.Description = EditorGUILayout.TextArea(ModPayload.Instance.Description);
		GUILayout.Space(10);
		if (GUILayout.Button ("Export Mod")) {
			Exporter.Export (ModPayload.Instance);
		}

	
	}
}


