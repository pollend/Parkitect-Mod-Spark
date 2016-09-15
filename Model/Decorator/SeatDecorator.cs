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


}


