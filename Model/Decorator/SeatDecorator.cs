using System;
using UnityEngine;
using System.Collections.Generic;

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

			seat1.transform.parent = parkitectObj.gameObject.transform;

			seat1.transform.localPosition = new Vector3(0, 0.1f, 0);
			seat1.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		if (GUILayout.Button("Create 2 Seat"))
		{
			GameObject seat1 = new GameObject("Seat");
			GameObject seat2 = new GameObject("Seat");

			seat1.transform.parent = parkitectObj.gameObject.transform;
			seat2.transform.parent = parkitectObj.gameObject.transform;

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
	#endif


}


