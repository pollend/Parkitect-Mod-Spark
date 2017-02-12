using UnityEditor;
using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class ApplyRotation : RideAnimationEvent 
{
    [SerializeField]
    public MultipleRotations rotator;
    float lastTime;


    public override string EventName
    {
        get
        {
            return "ApplyRotations";
        }
    }
    
	public override void RenderInspectorGUI(Motor[] motors)
    {

        if (rotator)
        {
            ColorIdentifier = rotator.ColorIdentifier;
        }
		foreach (MultipleRotations R in motors.OfType<MultipleRotations>().ToList())
        {
            if (R == rotator)
                GUI.color = Color.red / 1.3f;
            if (GUILayout.Button(R.Identifier))
            {
                rotator = R;

            }
            GUI.color = Color.white;
        }
		base.RenderInspectorGUI(motors);
    }

    public override void Enter()
    {
        
    }
	public override void Run(Transform root)
    {
        if (rotator)
        {


			rotator.tick(Time.realtimeSinceStartup - lastTime,root);
            lastTime = Time.realtimeSinceStartup;
            done = true;
			base.Run(root);
        }

    }
}
