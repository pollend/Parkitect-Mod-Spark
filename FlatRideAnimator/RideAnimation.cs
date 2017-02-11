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
			foreach (MultipleRotations R in motors.OfType<MultipleRotations>().ToList())
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
		foreach (Motor m in motors.OfType<Rotator>().ToList())
		{
			m.Enter();

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



