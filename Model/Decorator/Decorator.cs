﻿using System;
using UnityEngine;

[Serializable]
public class Decorator : ScriptableObject
{
	public Decorator ()
	{
	}

	public virtual void RenderSceneGUI(ParkitectObj parkitectObj){}
    public virtual void RenderInspectorGUI(ParkitectObj parkitectObj){}
}
