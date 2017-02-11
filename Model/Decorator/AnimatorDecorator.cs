﻿using System;
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
		this.phases.Add (phase);
	}

	public void RemovePhse(Phase phase)
	{
		this.phases.Remove (phase);
	}



	public void Animate()
	{
		foreach (Motor m in motors)
		{
			m.Enter();
		}
		if (phases.Count <= 0)
		{
			animating = false;
			foreach (Motor m in motors)
			{
				m.Reset();
			}
			foreach (MultipleRotations R in motors.OfType<MultipleRotations>())
			{
				R.Reset();
			}
			return;
		}
		foreach (Motor m in motors)
		{
			m.Enter();
		}

		animating = true;
		phaseNum = 0;
		currentPhase = phases[phaseNum];
		currentPhase.running = true;
		currentPhase.Enter();
		currentPhase.Run();
	}



	void NextPhase()
	{

		currentPhase.Exit();
		currentPhase.running = false;
		phaseNum++;
		if (phases.Count > phaseNum)
		{
			currentPhase = phases[phaseNum];
			currentPhase.running = true;
			currentPhase.Enter();
			currentPhase.Run();
			return;
		}
		animating = false;
		foreach (Motor m in motors.OfType<Rotator>())
		{
			m.Enter();

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
			m.axis.localPosition = m.originalRotationValue;

		}

		currentPhase = null;
	}
	public void Run()
	{
		if (currentPhase != null)
		{
			currentPhase.Run();
			if (!currentPhase.running)
			{
				NextPhase();
			}
		}
	}
}

