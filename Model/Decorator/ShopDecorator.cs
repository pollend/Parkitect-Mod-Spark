using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShopDecorator : Decorator
{
	public List<Product> products = new List<Product>();

	[NonSerialized]
	private Vector2 scrollPos2;
	[NonSerialized]
	private Product selected;

	public ShopDecorator ()
	{
	}


	#if UNITY_EDITOR
	public override void RenderInspectorGUI (ParkitectObj parkitectObj)
	{

		foreach (Product p in products)
		{
			try
			{
				EditorUtility.SetDirty(p);
			}
			catch 
			{
			}
		}
		Event e = Event.current;

		GUILayout.Space(10);
		EditorGUILayout.LabelField("Products:", EditorStyles.boldLabel);
		scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, "GroupBox", GUILayout.Height(100));
		for (int i = 0; i < products.Count; i++)
		{
			Color gui = GUI.color;
			if (products[i] == selected)
			{ GUI.color = Color.red; }

			if (GUILayout.Button(products[i].Name + "    $" + products[i].Price + ".00" + " --" + products[i].ToString()))
			{

				GUI.FocusControl("");
				if (e.button == 1)
				{
					GameObject.DestroyImmediate(products[i]);
					products.RemoveAt(i);
					return;
				}

				if (selected == products[i])
				{
					selected = null;
					return;
				}
				selected = products[i];
			}
			GUI.color = gui;
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Wearable Product"))
		{
			AddProduct<WearableProduct> ();
		}
		if (GUILayout.Button("Add Consumable Product"))
		{
			AddProduct<ConsumableProduct> ();
		}
		if (GUILayout.Button("Add OnGoing Product"))
		{
			AddProduct<OngoingProduct> ();
		}
		EditorGUILayout.EndHorizontal();
		if(selected != null)
		{
			if(!products.Contains(selected))
			{
				selected = null;
				return;
			}
			GUILayout.Space(10);
			selected.RenderInspectorGUI();

		}
		base.RenderInspectorGUI (parkitectObj);
	}
	#endif

	public void AddProduct<T>() where T : Product
	{
		Product product = ScriptableObject.CreateInstance<T> ();
		AssetDatabase.AddObjectToAsset (product,this);
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssets();
		products.Add (product);
	}

	public override void CleanUp (ParkitectObj parkitectObj)
	{
		for (int i = 0; i < products.Count; i++) {
			DestroyImmediate (products [i], true);
		}

		base.CleanUp (parkitectObj);
	}
}


