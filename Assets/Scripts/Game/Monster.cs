using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	void OnNewRoomEntered(Room newRoom)
	{
		foreach (var unit in newRoom.inhabitants)
			if (unit.tag == "Adventurer")
				Destroy (unit.gameObject);
	}
}
