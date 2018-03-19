using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModObjectsList
{
	private Vector2 scrollPos2;
	public ParkitectObj selectedParkitectObject 
	{ 
		get { return ModPayload.Instance.selectedParkitectObject;}
		private set { 
			ModPayload.Instance.selectedParkitectObject = value;
		}
	}

	public void SwitchParkitectObject(ParkitectObj orignal, ParkitectObj replace)
	{
		if (ModPayload.Instance.ParkitectObjs.Contains (orignal)) {
			ModPayload.Instance.ParkitectObjs.Remove (orignal);

			Object.DestroyImmediate (orignal, true);
			AssetDatabase.SaveAssets ();
			if(!SceneManager.GetActiveScene().isDirty)
				EditorSceneManager.MarkSceneDirty (SceneManager.GetActiveScene());
		
		}
		AddParkitectObject (replace);

		if (orignal == selectedParkitectObject) {
			selectedParkitectObject = replace;
		}
	}

	public bool RemoveParkitectObject(ParkitectObj obj)
	{
		if (obj == selectedParkitectObject)
			selectedParkitectObject = null;

		obj.CleanUp ();
        ModPayload.Instance.ParkitectObjs.Remove (obj);
		Object.DestroyImmediate (obj, true);
        obj.CleanUp ();
		AssetDatabase.SaveAssets ();
		if(!SceneManager.GetActiveScene().isDirty)
			EditorSceneManager.MarkSceneDirty (SceneManager.GetActiveScene());
		

		return ModPayload.Instance.ParkitectObjs.Remove (obj);
	}

	public bool AddParkitectObject(ParkitectObj obj)
	{
		if (!ModPayload.Instance.ParkitectObjs.Contains (obj)) {
			AssetDatabase.AddObjectToAsset (obj,ModPayload.Instance);
			EditorUtility.SetDirty(obj);
			AssetDatabase.SaveAssets ();

			if(!EditorSceneManager.GetActiveScene().isDirty)
				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene());

			ModPayload.Instance.ParkitectObjs.Add (obj);
			return true;
		}
		return false;
	}

	public void ClearParkitectObjects()
	{
		selectedParkitectObject = null;

        for (int x = 0; x < ModPayload.Instance.ParkitectObjs.Count; x++) {
            ModPayload.Instance.ParkitectObjs [x].CleanUp ();
            Object.DestroyImmediate (ModPayload.Instance.ParkitectObjs [x], true);
        }

		AssetDatabase.SaveAssets ();
		if(!EditorSceneManager.GetActiveScene().isDirty)
			EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene());
		

		ModPayload.Instance.ParkitectObjs.Clear ();
	}



	public void Render(Object[] selectedObjects)
	{
		Event e = Event.current;

		GUILayout.Label("Objects in mod", "PreToolbar");
		GUILayout.BeginHorizontal();
		GUILayout.EndHorizontal();
		scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, "GroupBox", GUILayout.Height(140));
		foreach (ParkitectObj PO in ModPayload.Instance.ParkitectObjs)
		{
			if (PO == selectedParkitectObject)
			{
				GUI.color = Color.red;
			}
			if (!PO.Prefab)
			{
				if (GUILayout.Button("##No game object found##", "minibutton"))
				{
					if (EditorUtility.DisplayDialog("Are you sure to delete this item?", "Are you sure to delete this missing GameObject item?  ", "Ok", "Cancel"))
					{
						RemoveParkitectObject (PO);
					}

					GUI.FocusControl("");
					//modWindow.selectedWaypoint = null;
				}
			}
			else
			{
				if (GUILayout.Button(PO.Prefab.name + " (" + PO.GetObjectTag() + ")", "minibutton"))
				{
					if (e.button == 1)
					{
						if (EditorUtility.DisplayDialog("Are you sure to delete this item?", "Are you sure to delete this item? Name: " + PO.Prefab.name, "Ok", "Cancel"))
						{
							PO.CleanUp ();
							RemoveParkitectObject (PO);
							//modManager.ParkitectObjects.Remove(PO);
							//modManager.asset = null;
						}
						return;
					}

					GUI.FocusControl("");

					if (PO is FlatRideParkitectObject && EditorWindow.GetWindow(typeof(FlatRideAnimator)))
					{
						EditorWindow.GetWindow(typeof(FlatRideAnimator)).Repaint();
					}

					//Disable editing
					//modWindow.enableEditing = false;
					//modWindow.BBW.enableEditing = false;

					if (selectedParkitectObject == PO)
					{
						selectedParkitectObject = null;

					}
					else
					{
						selectedParkitectObject = PO;

					}

					GameObject instance = PO.getGameObjectRef (true);

					EditorGUIUtility.PingObject(instance);
					GameObject[] newSelection = new GameObject[1];
					newSelection[0] = instance;
					Selection.objects = newSelection;
					PO.LoadDecorators ();


					if (SceneView.lastActiveSceneView)
					{
						SceneView.lastActiveSceneView.FrameSelected();
					}
					//modWindow.selectedWaypoint = null;

				}

				GUI.color = Color.white;
			}

		}

		EditorGUILayout.EndScrollView();


		GUILayout.BeginHorizontal();
		GUILayout.Label("Add Gameobject:", "GUIEditor.BreadcrumbLeft");

		GUILayout.Space(5);
		GameObject GO = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), true);
		if (GO)
		{

			foreach (ParkitectObj po in  ModPayload.Instance.ParkitectObjs)
			{
				if (po.Prefab == GO)
				{
					EditorUtility.DisplayDialog("This object is already in the list", "The object that you want to add to the list is already in the list", "Ok");
					EditorGUIUtility.PingObject(po.Prefab);
					return;
				}
				if (po.Prefab.name == GO.name) {
					if (!EditorUtility.DisplayDialog ("This object has the same name", "This object will replace the existing object if you want to proceede?", "Ok", "Cancel")) {
						EditorGUIUtility.PingObject(po.Prefab);
						return;
					}

				}
			}
			DecoParkitectObject decoParkitectObject = ScriptableObject.CreateInstance<DecoParkitectObject> (); //new ParkitectObj(new Decorator[]{});
			decoParkitectObject.SetGameObject (GO);

			float currentX = 0;
			AddParkitectObject (decoParkitectObject);
			selectedParkitectObject = decoParkitectObject;

		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5);

		if (selectedObjects.Length > 0)
		{
			if (GUILayout.Button("Add selection"))
			{
				foreach (var o in Selection.objects)
				{
					var gObj = (GameObject) o;
					foreach (ParkitectObj po in ModPayload.Instance.ParkitectObjs)
					{
						if (po.Prefab == gObj) {
							Debug.LogWarning ("Object already in list");
							EditorGUIUtility.PingObject (po.Prefab);
							return;
						}

						if (!GameObject.Find (po.Prefab.name)) {
							Debug.LogWarning ("Object not in scene");
							EditorGUIUtility.PingObject (po.Prefab);
							return;
						}

						if (gObj.name == po.Prefab.name) {
							Debug.LogWarning ("Object found with the same name");
							EditorGUIUtility.PingObject (po.Prefab);
							return;
						}


					}
					DecoParkitectObject emptyParkitectObject = ScriptableObject.CreateInstance<DecoParkitectObject> ();
					emptyParkitectObject.SetGameObject(gObj);
					AddParkitectObject (emptyParkitectObject);
					selectedParkitectObject = emptyParkitectObject;
	
				}

			}
		}

		GUILayout.Space(5);
		if (GUILayout.Button("Delete the list"))
		{
			if (EditorUtility.DisplayDialog("Are you sure to delete this list?", "Are you sure to delete this whole list ", "Ok", "Cancel"))
			{
				ClearParkitectObjects ();
			}
		}
		if (selectedParkitectObject != null && selectedParkitectObject.Prefab)
		{
			if (GUILayout.Button("Delete this object"))
			{
				if (EditorUtility.DisplayDialog("Are you sure to delete this item?", "Are you sure to delete this item? Name: " +  selectedParkitectObject.name, "Ok", "Cancel"))
				{
					RemoveParkitectObject (selectedParkitectObject);
				}
			}
		}
	}





}


