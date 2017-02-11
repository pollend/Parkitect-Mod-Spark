using UnityEditor;
using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class SpinRotater : RideAnimationEvent 
{
    [SerializeField]
    public Rotator rotator;
    [SerializeField]
    public bool spin = false;
    [SerializeField]
    public float spins = 1;
    float lastTime;


    public override string EventName
    {
        get
        {
            return "SpinRotator";
        }
    }
	public override void RenderInspectorGUI(AnimatorDecorator animator)
    {

        if (rotator)
        {
            ColorIdentifier = rotator.ColorIdentifier;
            spin = EditorGUILayout.Toggle("amountOfSpins ", spin);
            if (spin)
                spins = EditorGUILayout.FloatField("spins ", spins);

            EditorGUILayout.LabelField("Amount " + rotator.getRotationsCount());
        }
        
		foreach (Rotator R in animator.Motors.OfType<Rotator>().ToList())
        {
            if (R == rotator)
                GUI.color = Color.red / 1.3f;
            if (GUILayout.Button(R.Identifier))
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
        rotator.resetRotations();
        base.Enter();
    }
    public override void Run()
    {
        if (rotator)
        {


            rotator.tick(Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            if (spin)
            {
                if (rotator.getRotationsCount() >= spins)
                {
                    done = true;
                }
            }
            else
            { done = true;}
            
            base.Run();
        }

    }
}
