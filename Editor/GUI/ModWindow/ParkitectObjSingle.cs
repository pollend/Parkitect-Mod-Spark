using System;
using UnityEngine;
using UnityEditor;

public class ParkitectObjSingle
{
	public ParkitectObjSingle ()
	{
	}

	public void RenderInspectorGUI(ParkitectObj selectedObject)
	{

		if (selectedObject != null && selectedObject.Prefab != null) {

			GUILayout.BeginHorizontal("flow background");
			GUILayout.Label(selectedObject.Prefab.name, "LODLevelNotifyText");
			GUILayout.EndHorizontal();


			Type[] types =  selectedObject.SupportedDecorators ();
			for (int x = 0; x < types.Length; x++) {
                selectedObject.GetDecorator (types [x]).RenderInspectorGUI (selectedObject);
			}
		}
	}

    public void RenderSceneGUI(ParkitectObj selectedObject)
    {
        if (selectedObject != null && selectedObject.Prefab != null) {
            Type[] types =  selectedObject.SupportedDecorators ();
            for (int x = 0; x < types.Length; x++) {
                selectedObject.GetDecorator (types [x]).RenderSceneGUI (selectedObject);
            }
        }
    }
}


