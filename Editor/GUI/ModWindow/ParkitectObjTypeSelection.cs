using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class ParkitectObjTypeSelection
{
	[NonSerialized]
	string[] options = new string[]{};
	[NonSerialized]
	List<Type> parkitectObjs = new List<Type>();


	public ParkitectObjTypeSelection ()
	{
		IEnumerable<Assembly> scriptAssemblies = AppDomain.CurrentDomain.GetAssemblies ().Where ((Assembly assembly) => assembly.FullName.Contains ("Assembly"));
		List<String> options = new List<string> ();
		parkitectObjs.Clear ();
		foreach (Assembly assembly in scriptAssemblies) 
		{
			foreach (Type type in assembly.GetTypes().Where(T => T.IsClass && T.IsSubclassOf(typeof(ParkitectObj))))
			{
				object[] nodeAttributes = type.GetCustomAttributes(typeof(ParkitectObjectTag), false);                    
				ParkitectObjectTag attr = nodeAttributes[0] as ParkitectObjectTag;
				if (attr != null) {
					parkitectObjs.Add (type);
					options.Add (attr.Name);
				}
			}
		}
		this.options = options.ToArray ();
	}

	public void Render(ModObjectsList modObjectList)
	{
		if (modObjectList.selectedParkitectObject != null) {

			object[] attributes =  modObjectList.selectedParkitectObject.GetType ().GetCustomAttributes (typeof(ParkitectObjectTag), false);

			int current = 0;
			if (attributes.Length != 0) {
				current = Array.IndexOf (options, (System.Object)(attributes[0] as ParkitectObjectTag).Name);
			}

			GUILayout.BeginHorizontal ();
				int selection = EditorGUILayout.Popup ("Type", current, options); 
				if (selection != current) {
					
					modObjectList.selectedParkitectObject.CleanUp ();

					ParkitectObj pkObj= (ParkitectObj)ScriptableObject.CreateInstance (parkitectObjs [selection]);
					pkObj.Load (modObjectList.selectedParkitectObject);
					modObjectList.switchParkitectObject (modObjectList.selectedParkitectObject, pkObj);
				}
				GUILayout.EndHorizontal ();
			
		}
	}
}


