using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Collections.ObjectModel;

[ExecuteInEditMode]
[Serializable]
public class Phase : ScriptableObject
{
	[SerializeField]
	private List<RideAnimationEvent> events = new List<RideAnimationEvent>();
	public bool running = false;
	bool done = false;

	public  ReadOnlyCollection<RideAnimationEvent> Events{get{return events.AsReadOnly ();}}

	public void AddEvent(RideAnimationEvent animationEvent)
	{
		AssetDatabase.AddObjectToAsset (animationEvent,this);
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssets();
		events.Add (animationEvent);
	}

	public void DeleteEvent(RideAnimationEvent animationEvent)
	{
		events.Remove (animationEvent);
		DestroyImmediate (animationEvent, true);
	}

	public void CleanUp()
	{
		for(int x = 0; x < events.Count; x++)
		{
			if(events[x] != null)
				DestroyImmediate (events [x],true);
		}
	}

	public void Enter()
	{
		foreach (RideAnimationEvent RAE in events)
		{
			RAE.Enter();
		}
	}
	public Phase ShallowCopy()
	{
		return (Phase)this.MemberwiseClone();
	}
	public void Run(Transform root)
	{
		foreach (RideAnimationEvent RAE in events)
		{
			RAE.Run(root);
		}
		done = true;
		foreach (RideAnimationEvent RAE in events)
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
		foreach (RideAnimationEvent RAE in events)
		{
			RAE.Exit();
		}
	}

}