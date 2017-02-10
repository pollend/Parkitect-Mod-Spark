using System;
using UnityEngine;
using System.Collections.Generic;

public enum EffectTypes { HUNGER, THIRST, HAPPINESS, TIREDNESS, SUGARBOOST }

[Serializable]
public class Ingredient 
{

	[SerializeField]
	public string Name = "New Ingredient";
	[SerializeField]
	public float price = 1;
	[SerializeField]
	public float amount = 1;
	[SerializeField]
	public bool tweakable = true;
	[SerializeField]
	public List<Effect> effects = new List<Effect>();
}


[Serializable]
public class Effect
{
	[SerializeField]
	public EffectTypes Type = EffectTypes.HUNGER;
	[SerializeField]
	public float amount;
}