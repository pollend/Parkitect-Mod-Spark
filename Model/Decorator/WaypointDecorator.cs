using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaypointDecorator : Decorator
{
	[NonSerialized]
	private bool enableEditing = false;
	[NonSerialized]
	private float helperPlaneY = 0;
	[Serializable]
	public Waypoint selectedWaypoint;

	public List<Waypoint> waypoints = new List<Waypoint>();


	public WaypointDecorator ()
	{
	}

	public override void RenderInspectorGUI (ParkitectObj parkitectObj)
	{
		FlatrideDecorator flatRideDecorator = parkitectObj.GetDecorator (typeof(FlatrideDecorator));

		string caption = "Enable Editing Waypoints";
		if (enableEditing)
		{
			caption = "Disable Editing Waypoints";
		}
		if (enableEditing)
		{
			GUI.color = Color.green;
		}
		bool currentEnableEditing = enableEditing;
		if (GUILayout.Button(caption))
		{
			selectedWaypoint = null;
			enableEditing = !enableEditing;
		}
		if (enableEditing)
		{
			GUI.color = Color.white;
		}
		if (currentEnableEditing != enableEditing)
		{
			if (enableEditing)
			{
				currentTool = Tools.current;
				Tools.current = Tool.None;


			}
			else
			{
				Tools.current = currentTool;
			}
		}
		if (enableEditing)
		{
			GUILayout.Label("S - Snap to axis of connected waypoints");
			helperPlaneY = EditorGUILayout.FloatField("Helper Plane Y", helperPlaneY);

			if (GUILayout.Button("Generate outer grid"))
			{
				generateOuterGrid();
			}
			if (GUILayout.Button("(A)dd Waypoint"))
			{
				addWaypoint();
			}
			if (GUILayout.Button("Rotate 90°"))
			{
				rotateWaypoints();
			}
			if (GUILayout.Button("Clear all"))
			{
				ModManager.asset.waypoints.Clear();
			}
		}

		base.RenderInspectorGUI (parkitectObj);
	}

	public override void RenderSceneGUI (ParkitectObj parkitectObj)
	{
		FlatrideDecorator flatRideDecorator = parkitectObj.GetDecorator (typeof(FlatrideDecorator));


		if (enableEditing) {
			Vector3 topLeft = new Vector3(-flatRideDecorator.XSize / 2, helperPlaneY, flatRideDecorator.ZSize / 2) + parkitectObj.gameObject.transform.position;
			Vector3 topRight = new Vector3(flatRideDecorator.XSize / 2, helperPlaneY, flatRideDecorator.ZSize / 2) + parkitectObj.gameObject.transform.position;
			Vector3 bottomLeft = new Vector3(-flatRideDecorator.XSize / 2, helperPlaneY, -flatRideDecorator.ZSize / 2) + parkitectObj.gameObject.transform.position;
			Vector3 bottomRight = new Vector3(flatRideDecorator.XSize / 2, helperPlaneY, -flatRideDecorator.ZSize / 2) + parkitectObj.gameObject.transform.position;
		}

		base.RenderSceneGUI (parkitectObj);
	}

	private void generateOuterGrid(FlatrideDecorator flatRideDecorator)
	{
		float minX = -flatRideDecorator.XSize / 2;
		float maxX = flatRideDecorator.XSize / 2;
		float minZ = -flatRideDecorator.ZSize / 2;
		float maxZ = flatRideDecorator.ZSize / 2;
		for (int xi = 0; xi < Mathf.RoundToInt(maxX - minX); xi++)
		{
			for (int zi = 0; zi < Mathf.RoundToInt(maxZ - minZ); zi++)
			{
				float x = minX + xi;
				float z = minZ + zi;
				if (!(x == minX || x == maxX - 1) && !(z == minZ || z == maxZ - 1))
				{
					continue;
				}
				Waypoint newWaypoint = new Waypoint();
				newWaypoint.localPosition = new Vector3(x + 0.5f, helperPlaneY, z + 0.5f);
				newWaypoint.isOuter = true;
				//if (waypoints.waypoints.Count > 0) {
				//newWaypoint.connectedTo.Add(waypoints.waypoints.Count - 1);
				//}
				ModManager.asset.waypoints.Add(newWaypoint);
			}
		}

	}
}


