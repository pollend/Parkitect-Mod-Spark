using System;
using UnityEngine;
using UnityEditor;

public class FlatrideDecorator : Decorator
{
	public float Excitement;
	public float Intensity;
	public float Nausea;
	public float XSize = 1;
	public float ZSize = 1;
	public Vector3 closedAngleRetraints;

	public FlatrideDecorator ()
	{
	}

	public override void RenderInspectorGUI (ParkitectObj parkitectObj)
	{

		GUILayout.Space(10);
		GUILayout.Label("Rating", EditorStyles.boldLabel);
		this.Excitement = EditorGUILayout.Slider("Excitement (" + getRatingCategory(this.Excitement) + ")", this.Excitement, 0, 100);
		this.Intensity = EditorGUILayout.Slider("Intensity (" + getRatingCategory(this.Intensity) + ")", this.Intensity, 0, 100);
		this.Nausea = EditorGUILayout.Slider("Nausea (" + getRatingCategory(this.Nausea) + ")", this.Nausea, 0, 100);
		GUILayout.Space(10);
		this.closedAngleRetraints = EditorGUILayout.Vector3Field("Closed Restraints Angle", this.closedAngleRetraints);

		GUILayout.Space(10);
		GUI.color = Color.white;
		this.XSize = (float)EditorGUILayout.IntField("X", (int)Math.Floor(this.XSize));
		this.ZSize = (float)EditorGUILayout.IntField("Z", (int)Math.Floor(this.ZSize));

		base.RenderInspectorGUI (parkitectObj);
	}

	private string getRatingCategory(float ratingValue)
	{
		ratingValue /= 100f;
		if (ratingValue > 0.9f)
		{
			return "Very High";
		}
		if (ratingValue > 0.6f)
		{
			return "High";
		}
		if (ratingValue > 0.3f)
		{
			return "Medium";
		}
		return "Low";
	}
}


