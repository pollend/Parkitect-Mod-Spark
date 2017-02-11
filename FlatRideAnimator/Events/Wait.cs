﻿using UnityEngine;
using UnityEditor;
using System;
[Serializable]
[ExecuteInEditMode]
public class Wait : RideAnimationEvent
{
    [SerializeField]
    public float seconds;
    float timeLimit;
    public override string EventName
    {
        get
        {
            return "Wait";
        }
    }
	public override void RenderInspectorGUI(AnimatorDecorator animator)
    {
       
        seconds = EditorGUILayout.FloatField("Seconds", seconds);
        if (isPlaying)
        {
            GUILayout.Label("Time" + (timeLimit - Time.realtimeSinceStartup));
        }
		base.RenderInspectorGUI(animator);
    }

    public override void Enter()
    {
        
        timeLimit = Time.realtimeSinceStartup + seconds;
        base.Enter();
    }
    public override void Run()
    {
        if (Time.realtimeSinceStartup > timeLimit)
        {

            done = true;
        }
        else
        {

        }
        base.Run();
    }

}
