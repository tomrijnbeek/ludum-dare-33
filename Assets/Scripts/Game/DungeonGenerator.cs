using UnityEngine;
using System.Collections;

public class DungeonGenerator : MonoBehaviourBase {
	
	public GameObject roomPrefab;

	// Use this for initialization
	void Awake () {
		var xRadius = GameManager.Instance.xRadius;
		var yRadius = GameManager.Instance.yRadius;
		var map = RoomMap.Instance;

		for (int i = -xRadius; i <= xRadius; i++)
			for (int j = -yRadius; j <= yRadius; j++)
		{
			if (Random.value > .95 && (i != 0 || j != 0))
				continue;

			var room = Instantiate (roomPrefab);
			room.transform.position = new Vector3(i, j, 0);
			room.transform.parent = transform;
			room.name = string.Format("Room ({0},{1})",i,j);
			map.RegisterRoom(room.GetComponent<Room>(), true);
		}

		Destroy (this);
	}
}
