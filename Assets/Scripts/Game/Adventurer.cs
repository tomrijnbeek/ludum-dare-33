using UnityEngine;
using System.Collections;

public class Adventurer : MonoBehaviourBase {

	void OnNewRoomEntered(Room newRoom)
	{
		if (newRoom.ContainsUnit("Player"))
			GameManager.Instance.GameOver();

		Unit trapUnit;
		if (newRoom.TryGetUnit("Trap", out trapUnit))
		{
			Destroy (this.gameObject);
			Destroy (trapUnit.gameObject);
		}
	}
}
