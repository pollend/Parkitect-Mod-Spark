using System;
using UnityEngine;
using System.Collections.Generic;


[ExecuteInEditMode]
[Serializable]
public class Phase
{
	[SerializeField]
	public List<RideAnimationEvent> Events = new List<RideAnimationEvent>();
	public bool running = false;
	bool done = false;
	public void Enter()
	{
		foreach (RideAnimationEvent RAE in Events)
		{
			RAE.Enter();
		}
	}
	public Phase ShallowCopy()
	{
		return (Phase)this.MemberwiseClone();
	}
	public void Run()
	{
		foreach (RideAnimationEvent RAE in Events)
		{
			RAE.Run();
		}
		done = true;
		foreach (RideAnimationEvent RAE in Events)
		{
			if (!RAE.done)
			{
				running = true;
				done = false;
				break;
			}
		}
		if (done)
		{
			running = false;
		}

	}
	public void Exit()
	{
		foreach (RideAnimationEvent RAE in Events)
		{
			RAE.Exit();
		}
	}

}