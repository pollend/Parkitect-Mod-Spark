
using UnityEditor;
using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class FromToRot : RideAnimationEvent
{
    public RotateBetween rotator;

    float lastTime;
    public override string EventName
    {
        get
        {
            return "From-To Rot";
        }
    }
	public override void RenderInspectorGUI(AnimatorDecorator animator)
    {
        if (rotator)
        {
            ColorIdentifier = rotator.ColorIdentifier;
        }
		foreach (RotateBetween R in animator.Motors.OfType<RotateBetween>().ToList())
        {
            if (R == rotator)
                GUI.color = Color.red;
            if(GUILayout.Button(R.Identifier))
            {
                rotator = R;
            }
            GUI.color = Color.white;
        }
		base.RenderInspectorGUI(animator);
    }

    public override void Enter()
    {
        lastTime = Time.realtimeSinceStartup;

        rotator.startFromTo();
        base.Enter();
    }
    public override void Run()
    {

        if (rotator)
        {
            
            
            rotator.tick(Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            if (rotator.isStopped())
            {
                done = true;
            }
            base.Run();
        }
        
    }
}
