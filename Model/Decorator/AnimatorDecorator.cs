using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections.ObjectModel;


public class AnimatorDecorator : Decorator
{
	[SerializeField]
	private List<Motor> motors = new List<Motor>();

	[SerializeField]
	private List<Phase> phases = new List<Phase>();

	[SerializeField]
	public Phase currentPhase;
	int phaseNum;

	[SerializeField]
	public bool animating;

	public  ReadOnlyCollection<Motor> Motors{get{return motors.AsReadOnly ();}}

	public  ReadOnlyCollection<Phase> Phases{get{return phases.AsReadOnly ();}}

	public void AddMotor(Motor motor)
	{
		AssetDatabase.AddObjectToAsset (motor,this);
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssets();

		this.motors.Add (motor);
	}

	public void RemoveMotor(Motor motor)
	{
		this.motors.Remove (motor);
		DestroyImmediate (motor, true);
	}

	public void AddPhase(Phase phase)
	{
		AssetDatabase.AddObjectToAsset (phase,this);
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssets();

		this.phases.Add (phase);
	}

	public void RemovePhase(Phase phase)
	{
		this.phases.Remove (phase);
		DestroyImmediate (phase, true);
	}

	public override void CleanUp (ParkitectObj parkitectObj)
	{
		for (int x = 0; x < phases.Count; x++) {
			if (phases [x] != null) {
				phases [x].CleanUp ();
				DestroyImmediate (phases [x], true);
			}
		}
		for (int x = 0; x < motors.Count; x++) {
			if (motors [x] != null) {
				DestroyImmediate (motors [x], true);
			}
		}
		motors.Clear ();
		phases.Clear ();

		base.CleanUp (parkitectObj);
	}
		
	public void Animate(Transform root)
	{
		foreach (Motor m in motors)
		{
			m.Enter(root);
		}
		if (phases.Count <= 0)
		{
			animating = false;
			foreach (Motor m in motors)
			{
				m.Reset(root);
			}
			foreach (MultipleRotations R in motors.OfType<MultipleRotations>())
			{
				R.Reset(root);
			}
			return;
		}
		foreach (Motor m in motors)
		{
			m.Enter(root);
		}

		animating = true;
		phaseNum = 0;
		currentPhase = phases[phaseNum];
		currentPhase.running = true;
		currentPhase.Enter();
		currentPhase.Run(root);
	}



	void NextPhase(Transform root)
	{

		currentPhase.Exit();
		currentPhase.running = false;
		phaseNum++;
		if (phases.Count > phaseNum)
		{
			currentPhase = phases[phaseNum];
			currentPhase.running = true;
			currentPhase.Enter();
			currentPhase.Run(root);
			return;
		}
		animating = false;
		foreach (Motor m in motors.OfType<Rotator>())
		{
			m.Enter(root);

		}
		foreach (Rotator m in motors.OfType<Rotator>())
		{
			m.axis.localRotation = m.originalRotationValue;

		}
		foreach (RotateBetween m in motors.OfType<RotateBetween>())
		{
			m.axis.localRotation = m.originalRotationValue;

		}
		foreach (Mover m in motors.OfType<Mover>())
		{
			Transform transform =  m.axis.FindSceneRefrence (root);
			if(transform != null)
				transform.localPosition = m.originalRotationValue;

		}

		currentPhase = null;
	}
	public void Run(Transform transform)
	{
		if (currentPhase != null)
		{
			currentPhase.Run(transform);
			if (!currentPhase.running)
			{
				NextPhase(transform);
			}
		}
	}
}

