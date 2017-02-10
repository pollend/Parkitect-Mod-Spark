using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


public class ModObjectsList
{
	private Vector2 scrollPos2 = new Vector2();
	public ParkitectObj selectedParkitectObject 
	{ 
		get { return ModPayload.Instance.selectedParkitectObject;}
		private set { 
			ModPayload.Instance.selectedParkitectObject = value;
		}
	}

	public ModObjectsList ()
	{
	}

	public void switchParkitectObject(ParkitectObj orignal, ParkitectObj replace)
	{
		if (ModPayload.Instance.ParkitectObjs.Contains (orignal)) {
			ModPayload.Instance.ParkitectObjs.Remove (orignal);

			UnityEngine.Object.DestroyImmediate (orignal, true);
			AssetDatabase.SaveAssets ();
			if(!EditorSceneManager.GetActiveScene().isDirty)
				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene());
		
		}
		this.AddParkitectObject (replace);

		if (orignal == this.selectedParkitectObject) {
			this.selectedParkitectObject = replace;
		}
	}

	public bool RemoveParkitectObject(ParkitectObj obj)
	{
		if (obj == this.selectedParkitectObject)
			this.selectedParkitectObject = null;

		obj.CleanUp ();
        ModPayload.Instance.ParkitectObjs.Remove (obj);
		UnityEngine.Object.DestroyImmediate (obj, true);
        obj.CleanUp ();
		AssetDatabase.SaveAssets ();
		if(!EditorSceneManager.GetActiveScene().isDirty)
			EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene());
		

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
		this.selectedParkitectObject = null;

        for (int x = 0; x < ModPayload.Instance.ParkitectObjs.Count; x++) {
            ModPayload.Instance.ParkitectObjs [x].CleanUp ();
            UnityEngine.Object.DestroyImmediate (ModPayload.Instance.ParkitectObjs [x], true);
        }

		AssetDatabase.SaveAssets ();
		if(!EditorSceneManager.GetActiveScene().isDirty)
			EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene());
		

		ModPayload.Instance.ParkitectObjs.Clear ();
	}



	public void Render(UnityEngine.Object[] selectedObjects)
	{
		Event e = Event.current;

		GUILayout.Label("Objects in mod", "PreToolbar");
		GUILayout.BeginHorizontal();
		GUILayout.EndHorizontal();
		scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, "GroupBox", GUILayout.Height(140));
		foreach (ParkitectObj PO in ModPayload.Instance.ParkitectObjs)
		{
			if (PO == this.selectedParkitectObject)
			{
				GUI.color = Color.red;
			}
			if (!PO.Prefab)
			{
				if (GUILayout.Button("##No game object found##", "minibutton"))
				{
					if (EditorUtility.DisplayDialog("Are you sure to delete this item?", "Are you sure to delete this missing GameObject item?  ", "Ok", "Cancel"))
					{
						this.RemoveParkitectObject (PO);
					}

					GUI.FocusControl("");
					//modWindow.selectedWaypoint = null;
				}

			}
			else
			{

				if (GUILayout.Button(PO.name, "minibutton"))
				{
					if (e.button == 1)
					{
						if (EditorUtility.DisplayDialog("Are you sure to delete this item?", "Are you sure to delete this item? Name: " + PO.Prefab.name, "Ok", "Cancel"))
						{
							PO.CleanUp ();
							this.RemoveParkitectObject (PO);
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

					if (this.selectedParkitectObject == PO)
					{
						this.selectedParkitectObject = null;

					}
					else
					{
						this.selectedParkitectObject = PO;

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
			DecoParkitectObject deco = ScriptableObject.CreateInstance<DecoParkitectObject> (); //new ParkitectObj(new Decorator[]{});
			deco.SetGameObject (GO);

			float currentX = 0;
			this.AddParkitectObject (deco);
			selectedParkitectObject = deco;
			//modManager.ParkitectObjects.Add(PO);
			//modManager.asset = PO;
			foreach (ParkitectObj PObj in ModPayload.Instance.ParkitectObjs) {
				PObj.Prefab.transform.position = new Vector3 (currentX, 0, 0);
				currentX += PObj.XSize / 2;
			}



		}
		GUILayout.EndHorizontal();

		//if (modWindow.enableEditing)
		//{
		//	GUI.color = Color.grey;
		//}

		GUILayout.Space(5);

		if (selectedObjects.Length > 0)
		{
			if (GUILayout.Button("Add selection"))
			{
				foreach (GameObject GObj in Selection.objects)
				{
					foreach (ParkitectObj po in ModPayload.Instance.ParkitectObjs)
					{
						if (po.Prefab == GObj) {
							UnityEngine.Debug.LogWarning ("Object already in list");
							EditorGUIUtility.PingObject (po.Prefab);
							return;
						} else if (!GameObject.Find (po.Prefab.name)) {
							UnityEngine.Debug.LogWarning ("Object not in scene");
							EditorGUIUtility.PingObject (po.Prefab);
							return;
						} else if (GObj.name == po.Prefab.name) {
							UnityEngine.Debug.LogWarning ("Object found with the same name");
							EditorGUIUtility.PingObject (po.Prefab);
							return;
						}


					}
					DecoParkitectObject deco = ScriptableObject.CreateInstance<DecoParkitectObject> ();
					deco.SetGameObject(GObj);
					float currentX = 0;
					AddParkitectObject (deco);
					this.selectedParkitectObject = deco;
					//modManager.ParkitectObjects.Add(PO);
					//modManager.asset = PO;
					foreach (ParkitectObj PObj in ModPayload.Instance.ParkitectObjs)// modManager.ParkitectObjects)
					{
						PObj.Prefab.transform.position = new Vector3(currentX, 0, 0);
						currentX += PObj.XSize / 2;
					}
				}

			}
		}

		GUILayout.Space(5);
		if (GUILayout.Button("Delete the list"))
		{
			if (EditorUtility.DisplayDialog("Are you sure to delete this list?", "Are you sure to delete this whole list ", "Ok", "Cancel"))
			{
				this.ClearParkitectObjects ();
			}
		}
		if (selectedParkitectObject != null && selectedParkitectObject.Prefab)
		{
			if (GUILayout.Button("Delete this object"))
			{
				if (EditorUtility.DisplayDialog("Are you sure to delete this item?", "Are you sure to delete this item? Name: " +  this.selectedParkitectObject.name, "Ok", "Cancel"))
				{
					RemoveParkitectObject (this.selectedParkitectObject);
					//RemoveParkitectObject (selectedObjects);
					//modObjectPresenter.RemoveParkitectObject (modObjectPresenter.SelectedParkitectObject);
					//modManager.ParkitectObjects.Remove(modManager.asset);
					//modManager.asset = null;
				}
			}
		}
	}





}


