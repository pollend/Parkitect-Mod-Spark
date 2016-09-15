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
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Export XML"))
		{
			//change the payload
			//Exporter.SaveToXML(modManager);
		}
		GUILayout.EndHorizontal();
		//if (!modWindow.enableEditing)
		//{
	//		GUI.color = Color.green;
	//	}
		if (GUILayout.Button("Export MOD"))
		{
			//change the payload
			//Exporter.ExportMod(modManager);
		}
		//if (!modWindow.enableEditing)
		//{
		//		GUI.color = Color.white;
		//	}
		GUILayout.Space(10);
	}
}


