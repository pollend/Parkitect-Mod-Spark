
using UnityEditor;
using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class FromToMove : RideAnimationEvent
{
    public Mover rotator;

    float lastTime;
    public override string EventName
    {
        get
        {
            return "From-To Move";
        }
    }
	public override void RenderInspectorGUI(AnimatorDecorator animator)
    {
        if (rotator)
        {
            ColorIdentifier = rotator.ColorIdentifier;
        }
		foreach (Mover R in animator.Motors.OfType<Mover>().ToList())
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

        rotator.startFromTo();
        base.Enter();
    }
    public override void Run()
    {
        if (rotator)
        {
            rotator.tick(Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            if (rotator.reachedTarget())
            {
                done = true;
            }
            base.Run();
        }
        
    }
}
