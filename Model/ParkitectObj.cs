using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParkitectObj
{
	//Basic


	[SerializeField]
	private List<Decorator> decorators = new List<Decorator>();

	[SerializeField]
	public GameObject gameObject{ get; set; }
	[SerializeField]
	public string name{ get; set; }
	public float XSize;

	public ParkitectObj(List<Decorator> decorators)
	{
		this.decorators = decorators;
	}

	public ParkitectObj()
	{
		
	}


	public virtual Type[] SupportedDecorators()
	{
		return new Type[]{ };
	}

	public ParkitectObj (Decorator[] decorators)
	{
		this.decorators = new List<Decorator> ();
		for (int x = 0; x < decorators.Length; x++) {
			this.decorators.Add (decorators [x]);
		}
	}

	public Decorator GetDecorator(Type t)
	{
		for (int x = 0; x < decorators.Count; x++)
		{
			if (decorators [x].GetType().Equals(t)) {
				return decorators [x];
			}	
		}
		Decorator dec = (Decorator)Activator.CreateInstance(t);
		decorators.Add (dec);
		return dec;
		
	}
		
	public virtual void Load()
	{
	}

}

