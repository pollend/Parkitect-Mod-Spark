using UnityEditor;
using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class ChangePendulum : RideAnimationEvent 
{
    [SerializeField]
    public PendulumRotator rotator;
    float lastTime;
    public float Friction = 20f;
    public bool Pendulum;
    private float startPendulumPosition;
    private float windUpAngleTarget = 100f;
    public override string EventName
    {
        get
        {
            return "ChangePendulum";
        }
    }
    
	public override void RenderInspectorGUI(Motor[] motors)
    {

        if (rotator)
        {
            ColorIdentifier = rotator.ColorIdentifier;
            Friction = EditorGUILayout.FloatField("Friction", Friction);
            Pendulum = EditorGUILayout.Toggle("Pendulum", Pendulum);
        }
		foreach (PendulumRotator R in motors.OfType<PendulumRotator>().ToList())
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
        rotator.setActAsPendulum(Pendulum);
        rotator.angularFriction = Friction;
        done = true;
    }
	public override void Run(Transform root)
    {
        if (rotator)
        {
            
        }

    }
}
