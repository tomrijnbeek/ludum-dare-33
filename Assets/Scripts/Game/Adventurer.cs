using UnityEngine;
using System.Collections;

public class Adventurer : MonoBehaviourBase {

	public bool kill;

	void OnNewRoomEntered(Room newRoom)
	{
		if (newRoom.ContainsUnit("Player"))
			GameManager.Instance.GameOver();

		Unit trapUnit;
		if (newRoom.TryGetUnit("Trap", out trapUnit))
		{
			Kill ();
			Destroy (trapUnit.gameObject);
		}
	}

	void Update()
	{
		if (kill)
			Kill();
	}

	void Kill()
	{
		Destroy (this.gameObject);
		GameManager.Instance.QueueAdventurer();
		GameManager.Instance.QueueAdventurer();
	}
}
