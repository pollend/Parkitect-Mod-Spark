using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[Serializable]
[ExecuteInEditMode]
public class RideAnimation
{

	[SerializeField]
	public List<Motor> motors = new List<Motor>();

	[SerializeField]
	public List<Phase> phases = new List<Phase>();

	[SerializeField]
	public Phase currentPhase;
	int phaseNum;

	[SerializeField]
	public bool animating;
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
			foreach (MultipleRotations R in motors.OfType<MultipleRotations>().ToList())
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
		foreach (Motor m in motors.OfType<Rotator>().ToList())
		{
			m.Enter(root);

		}
		foreach (Rotator m in motors.OfType<Rotator>().ToList())
		{
			m.axis.localRotation = m.originalRotationValue;

		}
		foreach (RotateBetween m in motors.OfType<RotateBetween>().ToList())
		{
			m.axis.localRotation = m.originalRotationValue;

		}
		foreach (Mover m in motors.OfType<Mover>().ToList())
		{
			Transform transform =  m.axis.FindSceneRefrence (root);
			if(transform != null)
				transform.localPosition = m.originalRotationValue;

		}

		currentPhase = null;
	}
	public void Run(Transform root)
	{
		if (currentPhase != null)
		{
			currentPhase.Run(root);
			if (!currentPhase.running)
			{
				NextPhase(root);
			}
		}

	}
}



