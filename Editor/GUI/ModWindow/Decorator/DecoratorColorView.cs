using System;
using UnityEditor;
using UnityEngine;

public class DecoratorColorView : IDecoratorView		
{
	public DecoratorColorView() : base()
	{
	}


	public void Render(Decorator dec,ParkitectObj parkitectObj) 
	{
		ColorDecorator decorator = (ColorDecorator)dec;
		//ModManager.asset.Shader = (ParkitectObject.Shaders)EditorGUILayout.EnumPopup("Shader", ModManager.asset.Shader);
		//ModManager.asset.recolorable = EditorGUILayout.BeginToggleGroup("Recolorable", ModManager.asset.recolorable);

		//if (ModManager.asset.recolorable)
		{
			try
			{
				
				int colorsUsed = 0;
				decorator.color1 = EditorGUILayout.ColorField("Color 1", decorator.color1);
				if (decorator.color1 != new Color(0.95f, 0, 0))
				{
					colorsUsed = 1;
					decorator.color2 = EditorGUILayout.ColorField("Color 2", decorator.color2);
				}
				if (decorator.color2 != new Color(0.32f, 1, 0))
				{
					colorsUsed = 2;
					decorator.color3 = EditorGUILayout.ColorField("Color 3", decorator.color3);
				}
				if (decorator.color3 != new Color(0.110f, 0.059f, 1f))
				{
					colorsUsed = 3;
					decorator.color4 = EditorGUILayout.ColorField("Color 4", decorator.color4);
				}
				if(decorator.color4 != new Color(1, 0, 1))
					colorsUsed = 4;
				if(colorsUsed == 0)
				{
					GUILayout.Label("No custom colors used");
				}
				else if (colorsUsed == 1)
				{
					GUILayout.Label("You are only using color 1");
				}
				else if (colorsUsed == 2)
				{
					GUILayout.Label("You are only using color 1 & 2");
				}
				else if (colorsUsed == 3)
				{
					GUILayout.Label("You are only using color 1 - 3");
				}
				else if (colorsUsed == 4)
				{
					GUILayout.Label("You are only using color 1 - 4");
				}
				if (GUILayout.Button("Reset"))
				{
					decorator.color1 = new Color(0.95f, 0, 0);
					decorator.color2 = new Color(0.32f, 1, 0);
					decorator.color3 = new Color(0.110f, 0.059f, 1f);
					decorator.color4 = new Color(1, 0, 1);
				}
			}
			catch (Exception)
			{
			}
		}
		EditorGUILayout.EndToggleGroup();

	}
}


