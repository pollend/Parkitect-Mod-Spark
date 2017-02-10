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
	public ParkitectModManager ModManager;
	private bool enableEditing = false;
	Texture2D logo;
	private enum State
	{
		NONE, CONNECT
	}
	public Waypoint selectedWaypoint;

	Vector2 scrollPos = new Vector2();

	ModConfigurationView modConfigurationView = new ModConfigurationView();
	ModObjectsList  modObjectsView = new ModObjectsList();
	ParkitectObjTypeSelection parkitectObjTypeSelection = new ParkitectObjTypeSelection();
	ParkitectObjSingle parkitectObjSingle  = new ParkitectObjSingle();

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
		ModWindow window = (ModWindow)EditorWindow.GetWindow(typeof(ModWindow));
		window.Show();
		window.titleContent.text = "Parkitect Mod";
	}
	void OnEnable()
	{
		logo = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Parkitect_ModSpark/Textures/MSlogo.png", typeof(Texture2D));
		titleContent.image = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Parkitect_ModSpark/Textures/icon.png", typeof(Texture2D));
		//guiSkin = (GUISkin)(AssetDatabase.LoadAssetAtPath("Assets/Parkitect_ModSpark/Editor/GUI/GUISkin_Spark.guiskin", typeof(GUISkin)));
		//UpdateInfo.Check(false);
	}
	void OnSelectionChange()
	{
		Repaint();

	}

	void OnGUI()
	{
		ModManager.ValidateSelected();
		GUI.changed = false;
		if (!ModManager)
		{
			if (FindObjectOfType<ParkitectModManager>())
			{
				ModManager = FindObjectOfType<ParkitectModManager>();
			}
			else
			{
				if (GameObject.Find("ParkitectModManager"))
				{
					ModManager = (GameObject.Find("ParkitectModManager")).AddComponent<ParkitectModManager>();
				}
				else
				{

					ModManager = (new GameObject("ParkitectModManager")).AddComponent<ParkitectModManager>();
				}
			}
		}
		if (ModManager.asset != null)
		{
			if (!ModManager.asset.gameObject)
			{
				ModManager.asset = null;
			}
		}
		if (enableEditing)
		{
			GUI.color = Color.grey;
		}     
		//Show Update 
		if (UpdateInfo.CurNewVersion > UpdateInfo.CurVerion)
		{
			GUI.color = Color.red / 1.4f;
			GUILayout.BeginHorizontal("IN BigTitle");
			GUI.color = Color.white;
			GUILayout.Label("Version " + UpdateInfo.CurNewVersion + " is out! You have version " + UpdateInfo.CurVerion);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Update")) { Application.OpenURL(UpdateInfo.NewSite);}
			GUILayout.EndHorizontal();
		}
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		//Show logo
		var centeredStyle = GUI.skin.GetStyle("Label");
		centeredStyle.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label(logo, centeredStyle, GUILayout.MaxWidth(Screen.width), GUILayout.ExpandHeight(false));
		//Show Mod name
		GUILayout.BeginHorizontal("flow background");
		GUILayout.Label(ModManager.mod.name, "LODLevelNotifyText");
		GUILayout.EndHorizontal();

		Event e = Event.current;
		modConfigurationView.Render ();
		modObjectsView.Render (Selection.objects);

		if (ModManager.asset != null && ModManager.asset.gameObject)
		{

		}
		if (enableEditing)
		{
			GUI.color = Color.grey;
		}

		parkitectObjTypeSelection.Render (modObjectsView);
		parkitectObjSingle.RenderInspectorGUI (modObjectsView.selectedParkitectObject);

		EditorUtility.SetDirty(ModManager);
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
        if(parkitectObjSingle != null)
            parkitectObjSingle.RenderSceneGUI (modObjectsView.selectedParkitectObject);

	}

}




