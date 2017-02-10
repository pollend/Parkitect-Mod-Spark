using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Product : ScriptableObject
{
	public enum Tempreature
	{
		NONE,
		COLD,
		HOT
	}

	public List<Ingredient> ingredients = new List<Ingredient>();


	public string Name;
	public float Price;
	private Vector2 scrollPos = Vector2.zero;
	private Ingredient selected = null;

	public Product ()
	{
	}

	public virtual void RenderInspectorGUI(){
		Name = EditorGUILayout.TextField ("Name",Name);
		Price = EditorGUILayout.FloatField("Price ", Price);
		DrawIngredients ();
	}

	private void DrawIngredients()
	{
		Event e = Event.current;
		EditorGUILayout.LabelField("Ingredients:", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal(GUILayout.Height(300));
		EditorGUILayout.BeginVertical("ShurikenEffectBg", GUILayout.Width(150));
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));

		for (int i = 0; i < ingredients.Count; i++)
		{
			Color gui = GUI.color;
			if (ingredients[i] == selected)
			{ GUI.color = Color.red; }

			if (GUILayout.Button(ingredients[i].Name + "    $" + ingredients[i].price + ".00", "ShurikenModuleTitle"))
			{

				GUI.FocusControl("");
				if (e.button == 1)
				{
					ingredients.RemoveAt(i);
					return;
				}

				if (selected == ingredients[i])
				{
					selected = null;
					return;
				}
				selected = ingredients[i];
			}
			GUI.color = gui;
		}
		EditorGUILayout.EndScrollView();

		if (GUILayout.Button("Add Ingredients"))
		{
			ingredients.Add(new Ingredient());
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical();
		if(selected != null)
		{
			if (!ingredients.Contains(selected))
			{
				selected = null;
				return;
			}
			selected.Name = EditorGUILayout.TextField("Ingridient Name ", selected.Name);
			selected.price = EditorGUILayout.FloatField("Price ", selected.price);
			selected.amount = EditorGUILayout.FloatField("Amount ", selected.amount);
			selected.tweakable = EditorGUILayout.Toggle("Tweakable ", selected.tweakable);

			for (int i = 0; i < selected.effects.Count; i++)
			{
				Color gui = GUI.color;

				if (GUILayout.Button( "Effector " + selected.effects[i].Type, "ShurikenModuleTitle"))
				{

					GUI.FocusControl("");
					if (e.button == 1)
					{
						selected.effects.RemoveAt(i);
						return;
					}
				}

				selected.effects[i].Type = (EffectTypes)EditorGUILayout.EnumPopup("Type ", selected.effects[i].Type);
				selected.effects[i].amount = EditorGUILayout.Slider("Amount", selected.effects[i].amount, 1f, -1f);
				GUI.color = gui;
			}
			if (GUILayout.Button("Add Effect"))
			{
				selected.effects.Add(new Effect());
			}

		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndVertical();
	}

}


