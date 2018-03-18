using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;


public class ModWindow : EditorWindow
{
	public static string version = "APLHA v3.0.0";
	public GUISkin guiSkin;
	private bool enableEditing = false;
	Texture2D logo;

	private enum State
	{
		NONE,
		CONNECT
	}

	public SPWaypoint selectedWaypoint;

	Vector2 scrollPos = new Vector2();

	ModConfigurationView modConfigurationView = new ModConfigurationView();
	ModObjectsList modObjectsView = new ModObjectsList();
	ParkitectObjTypeSelection parkitectObjTypeSelection = new ParkitectObjTypeSelection();

	public void repaintWindow()
	{
		Repaint();
	}


	[MenuItem("Parkitect/Report Problem", false, 112)]
	static void ReportIssue()
	{
		Application.OpenURL("https://github.com/ParkitectNexus/Parkitect-Mod-Spark/issues/new");
	}

	[MenuItem("Parkitect/Wiki", false, 111)]
	static void OpenWiki()
	{
		Application.OpenURL("https://parkitectnexus.com/modding-wiki/Mod-Spark");
	}

	[MenuItem("Parkitect/Mod-Setup", false, 1)]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		ModWindow window = (ModWindow) EditorWindow.GetWindow(typeof(ModWindow));
		window.Show();
		window.titleContent.text = "Parkitect Mod";
	}

	void OnEnable()
	{
		logo = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Parkitect_ModSpark/Textures/MSlogo.png", typeof(Texture2D));
		titleContent.image =
			(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Parkitect_ModSpark/Textures/icon.png", typeof(Texture2D));
		//guiSkin = (GUISkin)(AssetDatabase.LoadAssetAtPath("Assets/Parkitect_ModSpark/Editor/GUI/GUISkin_Spark.guiskin", typeof(GUISkin)));
		//UpdateInfo.Check(false);
	}

	void OnSelectionChange()
	{
		Repaint();

	}

	void OnGUI()
	{

		if (enableEditing)
		{
			GUI.color = Color.grey;
		}

		//Show Update 
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		//Show logo
		var centeredStyle = GUI.skin.GetStyle("Label");
		centeredStyle.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label(logo, centeredStyle, GUILayout.MaxWidth(Screen.width), GUILayout.ExpandHeight(false));
		//Show Mod name
		GUILayout.BeginHorizontal("flow background");
		GUILayout.Label(ModPayload.Instance.modName, "LODLevelNotifyText");
		GUILayout.EndHorizontal();

		Event e = Event.current;
		modConfigurationView.Render();
		modObjectsView.Render(Selection.objects);


		if (enableEditing)
		{
			GUI.color = Color.grey;
		}

		parkitectObjTypeSelection.Render(modObjectsView);
		if (modObjectsView.selectedParkitectObject != null && modObjectsView.selectedParkitectObject.Prefab != null)
		{
			GUILayout.BeginHorizontal("flow background");
			GUILayout.Label(modObjectsView.selectedParkitectObject.Prefab.name, "LODLevelNotifyText");
			GUILayout.EndHorizontal();

			GUILayout.Label(modObjectsView.selectedParkitectObject.Key);
			if (GUILayout.Button("Update Prefab"))
			{
				modObjectsView.selectedParkitectObject.UpdatePrefab();
			}

			if (GUILayout.Button("Create Instance In Scene"))
			{
				modObjectsView.selectedParkitectObject.getGameObjectRef(true);
				modObjectsView.selectedParkitectObject.UpdatePrefab();
			}

			Type[] types = modObjectsView.selectedParkitectObject.SupportedDecorators();
			for (int x = 0; x < types.Length; x++)
			{
				var decorator = modObjectsView.selectedParkitectObject.GetDecorator(types[x], true);
				if (decorator != null)
					decorator.RenderInspectorGui(modObjectsView.selectedParkitectObject);
			}
		}

		EditorUtility.SetDirty(ModPayload.Instance);
		GUILayout.Space(50);
		EditorGUILayout.EndScrollView();

		GUILayout.Label("© H-POPS - " + version, "PreToolbar");

	}



	void OnFocus()
	{
		// Remove delegate listener if it has previously
		// been assigned.
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		// Add (or re-add) the delegate.
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

	void OnDestroy()
	{
		// When the window is destroyed, remove the delegate
		// so that it will no longer do any drawing.
		//Exporter.SaveToXML(ModManager);
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;

	}

	void OnSceneGUI(SceneView sceneView)
	{

		if (modObjectsView.selectedParkitectObject != null && modObjectsView.selectedParkitectObject.Prefab != null)
		{
			Type[] types = modObjectsView.selectedParkitectObject.SupportedDecorators();
			for (int x = 0; x < types.Length; x++)
			{
				var decorator = modObjectsView.selectedParkitectObject.GetDecorator(types[x], true);
				if (decorator != null)
					decorator.RenderSceneGui(modObjectsView.selectedParkitectObject);
			}
		}
	}

}




