using System;
using UnityEngine;
using UnityEditor;

public class ModConfigurationView
{


	public void Render()
	{
		GUILayout.Label("Mod setup", "PreToolbar");
		GUILayout.Label("Mod Name:", EditorStyles.boldLabel);
		ModPayload.Instance.modName = EditorGUILayout.TextField(ModPayload.Instance.modName);
		GUILayout.Label("Mod Description:", EditorStyles.boldLabel);
		ModPayload.Instance.description = EditorGUILayout.TextArea(ModPayload.Instance.description);
		GUILayout.Space(10);
		if (GUILayout.Button ("Export Mod")) {
			Exporter.Export (ModPayload.Instance);
		}

	
	}
}


