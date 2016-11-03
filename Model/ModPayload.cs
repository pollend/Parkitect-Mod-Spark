using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModPayload : ScriptableSingleton<ModPayload>
{
	[SerializeField]
	public List<ParkitectObj> parkitectObjecst;


	[SerializeField]
	public string name;
	[SerializeField]
	public string description ;

	public ModPayload ()
	{
		if (parkitectObjecst == null)
			parkitectObjecst = new List<ParkitectObj> ();
	}
}

