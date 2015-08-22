using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviourBase {

	void OnNewRoomEntered(Room newRoom)
	{
		foreach (var unit in newRoom.inhabitants)
			if (unit.tag == "Adventurer")
		{
			if (unit.direction != (GetComponent<Unit>().direction + 2) % 4)
				Destroy (unit.gameObject);
			else
				GameManager.Instance.GameOver();
		}
	}
}
