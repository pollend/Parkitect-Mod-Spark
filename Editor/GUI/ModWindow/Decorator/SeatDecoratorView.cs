using System;
using UnityEngine;

public class SeatDecoratorView : IDecoratorView
{
	ModObjectsList modObjectList;
	public SeatDecoratorView (ModObjectsList modObjectList)
	{
		this.modObjectList = modObjectList;
	}

	public void Render (Decorator decorator,ParkitectObj parkitectObj)
	{
		SeatDecorator seatDecorator = (SeatDecorator)decorator;

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Create 1 Seats"))
		{
			GameObject seat1 = new GameObject("Seat");

			seatDecorator.AddSeat (seat1);

			seat1.transform.parent = modObjectList.selectedParkitectObject.gameObject.transform;

			seat1.transform.localPosition = new Vector3(0, 0.1f, 0);
			seat1.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		if (GUILayout.Button("Create 2 Seat"))
		{
			GameObject seat1 = new GameObject("Seat");
			GameObject seat2 = new GameObject("Seat");

			seat1.transform.parent = modObjectList.selectedParkitectObject.gameObject.transform;
			seat2.transform.parent = modObjectList.selectedParkitectObject.gameObject.transform;

			seatDecorator.AddSeat (seat1);
			seatDecorator.AddSeat (seat2);

			seat1.transform.localPosition = new Vector3(0.1f, 0.1f, 0);
			seat1.transform.localRotation = Quaternion.Euler(Vector3.zero);
			seat2.transform.localPosition = new Vector3(-0.1f, 0.1f, 0);
			seat2.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		GUILayout.EndHorizontal();
	}
}


