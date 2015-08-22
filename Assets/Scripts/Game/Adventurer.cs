using UnityEngine;
using System.Collections;

public class Adventurer : MonoBehaviourBase {

	void OnNewRoomEntered(Room newRoom)
	{
		if (newRoom.ContainsUnit("Player"))
			GameManager.Instance.GameOver();
	}
}
