
using UnityEditor;
using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class StartRotator : RideAnimationEvent
{
    public Rotator rotator;
    float lastTime;
    public override string EventName
    {
        get
        {
            return "StartRotator";
        }
    }
	public override void RenderInspectorGUI(AnimatorDecorator animator)
    {
        if (rotator)
        {
            ColorIdentifier = rotator.ColorIdentifier;
        }
		foreach (Rotator R in animator.Motors.OfType<Rotator>().ToList())
        {
            if (R == rotator)
                GUI.color = Color.red / 1.3f;
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

        rotator.start();
        base.Enter();
    }
    public override void Run()
    {
        if (rotator)
        {
            
            rotator.tick(Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            if (rotator.reachedFullSpeed())
            {
                done = true;
            }
            base.Run();
        }
        
    }
}
