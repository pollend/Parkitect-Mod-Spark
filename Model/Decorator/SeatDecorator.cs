using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class SeatDecorator : Decorator
{
	[SerializeField]
	private GameObject[] seats;

	public void AddSeat(GameObject gameobject)
	{
		if (seats == null)
			seats = new GameObject[]{gameobject};
		else 
		{
			List<GameObject> temp = new List<GameObject> (seats);
			temp.Add (gameobject);
			seats = temp.ToArray ();
		}
		
	}

	public GameObject[] GetSeats()
	{
		List<GameObject> temp = new List<GameObject> ();
		for (int x = 0; x < seats.Length; x++) {
			if (seats [x] != null)
				temp.Add (seats [x]);
		}

		return temp.ToArray();
	}

	#if UNITY_EDITOR
    public override void RenderInspectorGUI (ParkitectObj parkitectObj)
	{
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Create 1 Seats"))
		{
			GameObject seat1 = new GameObject("Seat");

			this.AddSeat (seat1);

			seat1.transform.parent = parkitectObj.GameObjectRef.transform;

			seat1.transform.localPosition = new Vector3(0, 0.1f, 0);
			seat1.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		if (GUILayout.Button("Create 2 Seat"))
		{
			GameObject seat1 = new GameObject("Seat");
			GameObject seat2 = new GameObject("Seat");


			seat1.transform.parent = parkitectObj.GameObjectRef.transform;
			seat2.transform.parent = parkitectObj.GameObjectRef.transform;

			this.AddSeat (seat1);
			this.AddSeat (seat2);

			seat1.transform.localPosition = new Vector3(0.1f, 0.1f, 0);
			seat1.transform.localRotation = Quaternion.Euler(Vector3.zero);
			seat2.transform.localPosition = new Vector3(-0.1f, 0.1f, 0);
			seat2.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		GUILayout.EndHorizontal();

        base.RenderInspectorGUI (parkitectObj);
	}

	public override void RenderSceneGUI (ParkitectObj parkitectObj)
	{
		if(seats != null)
		for (int x = 0; x < seats.Length; x++) {
				if (seats [x] != null) {
					var transform = seats [x].transform;

					Handles.SphereCap (200, transform.position, transform.rotation, 0.05f);
					Vector3 vector = transform.position - transform.up * 0.02f + transform.forward * 0.078f - transform.right * 0.045f;
					Handles.SphereCap (201, vector, transform.rotation, 0.03f);
					Vector3 vector2 = transform.position - transform.up * 0.02f + transform.forward * 0.078f + transform.right * 0.045f;
					Handles.SphereCap (202, vector2, transform.rotation, 0.03f);
					Vector3 center = transform.position + transform.up * 0.305f + transform.forward * 0.03f;
					Handles.SphereCap (203, center, transform.rotation, 0.1f);
					Vector3 center2 = vector + transform.forward * 0.015f - transform.up * 0.07f;
					Handles.SphereCap (204, center2, transform.rotation, 0.02f);
					Vector3 center3 = vector2 + transform.forward * 0.015f - transform.up * 0.07f;
					Handles.SphereCap (205, center3, transform.rotation, 0.02f);
				}
		}

	
		base.RenderSceneGUI (parkitectObj);
	}
	#endif


}


