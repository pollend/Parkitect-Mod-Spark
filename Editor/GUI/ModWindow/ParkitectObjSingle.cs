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

		if (selectedObject != null && selectedObject.gameObject != null) {

			GUILayout.BeginHorizontal("flow background");
			GUILayout.Label(selectedObject.gameObject.name, "LODLevelNotifyText");
			GUILayout.EndHorizontal();

			Type[] types =  selectedObject.SupportedDecorators ();
			for (int x = 0; x < types.Length; x++) {
                selectedObject.GetDecorator (types [x]).RenderInspectorGUI (selectedObject);
			}
		}
	}

    public void RenderSceneGUI(ParkitectObj selectedObject)
    {
        if (selectedObject != null && selectedObject.gameObject != null) {
            Type[] types =  selectedObject.SupportedDecorators ();
            for (int x = 0; x < types.Length; x++) {
                selectedObject.GetDecorator (types [x]).RenderSceneGUI (selectedObject);
            }
        }
    }
}


