using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModPayload : ScriptableSingleton<ModPayload>
{
	[SerializeField]
	public List<ParkitectObj> ParkitectObjs;

	[SerializeField]
	public ParkitectObj selectedParkitectObject { get; set; }

	[SerializeField]
	public string name;
	[SerializeField]
	public string description ;

	public ModPayload ()
	{
		if (ParkitectObjs == null)
			ParkitectObjs = new List<ParkitectObj> ();
	}
}

