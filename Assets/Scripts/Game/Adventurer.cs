using UnityEngine;
using System.Collections;

public class Adventurer : MonoBehaviourBase {

	void OnNewRoomEntered(Room newRoom)
	{
		foreach (var unit in newRoom.inhabitants)
			if (unit.tag == "Player")
				GameManager.Instance.GameOver();
	}
}
