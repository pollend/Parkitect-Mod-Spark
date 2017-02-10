using UnityEngine;
using System;
using UnityEditor;

[Serializable]
public class WearableProduct : Product
{
	public enum Seasonal{WINTER, SPRING, SUMMER, AUTUMN, NONE}
	public enum Body { HEAD, FACE, BACK }

	[SerializeField]
	public Body BodyLocation  = Body.HEAD;
	[SerializeField]
	public Seasonal SeasonalPrefrence  = Seasonal.NONE;
	public Tempreature TempreaturePrefrence = Tempreature.NONE;
	public bool HideOnRide = false;
	public bool HideHair = false;



	public override void RenderInspectorGUI()
	{
		BodyLocation = (Body)EditorGUILayout.EnumPopup("Body Location ", BodyLocation);
		SeasonalPrefrence = (Seasonal)EditorGUILayout.EnumPopup ("Seasonal Prefrence ", SeasonalPrefrence);
		TempreaturePrefrence = (Tempreature)EditorGUILayout.EnumPopup ("Tempreature Prefrence", TempreaturePrefrence);
		HideHair = EditorGUILayout.Toggle("Hide Hair", HideHair);
		HideOnRide = EditorGUILayout.Toggle ("Hide On Ride", HideOnRide);
		base.RenderInspectorGUI();
	}


}