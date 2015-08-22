using UnityEngine;
using System.Collections;

public class DungeonGenerator : MonoBehaviourBase {

	public int xRadius = 9;
	public int yRadius = 4;

	public GameObject roomPrefab;

	// Use this for initialization
	void Awake () {
		var map = RoomMap.Instance;

		for (int i = -xRadius; i <= xRadius; i++)
			for (int j = -yRadius; j <= yRadius; j++)
		{
			if (Random.value > .95)
				continue;

			var room = Instantiate (roomPrefab);
			room.transform.position = new Vector3(i, j, 0);
			map.RegisterRoom(room.GetComponent<Room>(), true);
		}

		Destroy (this);
	}
}
