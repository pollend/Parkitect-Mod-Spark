using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class ColorDecorator : Decorator
{

	public ColorDecorator ()
	{
	}

	#if UNITY_EDITOR
	public override void Render (ParkitectObj parkitectObj)
	{
		//ModManager.asset.Shader = (ParkitectObject.Shaders)EditorGUILayout.EnumPopup("Shader", ModManager.asset.Shader);
		//ModManager.asset.recolorable = EditorGUILayout.BeginToggleGroup("Recolorable", ModManager.asset.recolorable);

		//if (ModManager.asset.recolorable)
		{
			try
			{

				int colorsUsed = 0;
				this.color1 = EditorGUILayout.ColorField("Color 1", this.color1);
				if (this.color1 != new Color(0.95f, 0, 0))
				{
					colorsUsed = 1;
					this.color2 = EditorGUILayout.ColorField("Color 2", this.color2);
				}
				if (this.color2 != new Color(0.32f, 1, 0))
				{
					colorsUsed = 2;
					this.color3 = EditorGUILayout.ColorField("Color 3", this.color3);
				}
				if (this.color3 != new Color(0.110f, 0.059f, 1f))
				{
					colorsUsed = 3;
					this.color4 = EditorGUILayout.ColorField("Color 4", this.color4);
				}
				if(this.color4 != new Color(1, 0, 1))
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
					this.color1 = new Color(0.95f, 0, 0);
					this.color2 = new Color(0.32f, 1, 0);
					this.color3 = new Color(0.110f, 0.059f, 1f);
					this.color4 = new Color(1, 0, 1);
				}
			}
			catch (Exception)
			{
			}
		}
		EditorGUILayout.EndToggleGroup();

		base.Render (parkitectObj);
	}
	#endif

	public Color color1 = new Color(0.95f, 0, 0);
	public Color color2 = new Color(0.95f, 0, 0);
	public Color color3 = new Color(0.95f, 0, 0);
	public Color color4 = new Color(0.95f, 0, 0);

}

