using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviourBase {

	public int amountOfRooms;
	int roomsCreated;

	RoomMap map;

	public GameObject roomPrefab;

	// Use this for initialization
	void Awake () {
		map = RoomMap.Instance;
		//StartCoroutine(Generate ());
		Generate ();
	}

	void Generate()
	{
		//const float delay = .01f;

		var roomsToExpandFrom = new Queue<Room>();
		var room = CreateRoomAt (0,0);
		roomsToExpandFrom.Enqueue(room);

		//yield return new WaitForSeconds(delay);

		while (roomsCreated < amountOfRooms)
		{
			if (roomsToExpandFrom.Count == 0)
			{
				// Just requeue 20 queues randomly.
				for (int i = 0; i < 20; i++)
				{
					if ((room = map.rooms[Random.Range(0, GameManager.Instance.xRadius * 2 + 1), Random.Range(0, GameManager.Instance.yRadius * 2 + 1)]) != null
					    && room.RandomEmptyDirection() != null) {
						roomsToExpandFrom.Enqueue(room);
					}
					continue;
				}
			}

			room = roomsToExpandFrom.Dequeue();

			// 50% chance to keep this in a corridor, unless it's the only room we have
//			if (roomsToExpandFrom.Count > 0 && Random.value < .5)
//				continue;

			var dirMaybe = room.RandomEmptyDirection();
			if (dirMaybe == null)
				continue;
			var dir = dirMaybe.Value;

			var coordinates = room.transform.position + Unit.Dir2Vector(dir);
			var x = Mathf.RoundToInt(coordinates.x);
			var y = Mathf.RoundToInt(coordinates.y);
			map.ShiftCoordinates(ref x, ref y);

			while (IsContained(coordinates) && map.GetRoomAt(coordinates) == null && ProbabilisticallySurrounded(coordinates))
			{
				var newRoom = CreateRoomAt(Mathf.RoundToInt(coordinates.x),Mathf.RoundToInt(coordinates.y));
				room.Connect(newRoom, dir);

				roomsToExpandFrom.Enqueue(newRoom);
				room = newRoom;

				dirMaybe = room.RandomEmptyDirection();
				if (dirMaybe == null)
					break;
				dir = dirMaybe.Value;

				coordinates = room.transform.position + Unit.Dir2Vector(dir);
				x = Mathf.RoundToInt(coordinates.x);
				y = Mathf.RoundToInt(coordinates.y);
				map.ShiftCoordinates(ref x, ref y);

				//yield return new WaitForSeconds(delay);
			}
		}

		// Make spawn room as connected as possible
		var spawnRoom = map.GetRoomAt(Vector3.zero);
		for (int i = 0; i < 4; i++)
		{
			if (spawnRoom.connections[i] == null && (room = map.GetRoomAt(Unit.Dir2Vector(i))) != null)
				spawnRoom.Connect(room, i);
		}

		//yield return new WaitForSeconds(delay);

		// Connect dead ends to another room
		for (int j = 0; j < map.rooms.GetLength(1); j++)
			for (int i = 0; i < map.rooms.GetLength(0); i++)
		{
			room = map.rooms[i,j];

			if (room == null || room.connections.Count(r => r != null) > 1 || Random.value < .05)
				continue;

			int tries = 10;
			while (tries --> 0)
			{
				var dir = room.RandomEmptyDirection().Value;
				var coordinates = room.transform.position + Unit.Dir2Vector(dir);
				if (!IsContained(coordinates) || map.GetRoomAt(coordinates) == null)
					continue;

				room.Connect(map.GetRoomAt(coordinates), dir);
				//yield return new WaitForSeconds(delay);
				break;
			}
		}

		Destroy (this);
	}

	bool ProbabilisticallySurrounded(Vector3 position)
	{
		var p = -.5;
		for (int i = 0; i < 4; i++)
			if (IsContained(position + Unit.Dir2Vector(i)) && map.GetRoomAt(position + Unit.Dir2Vector(i)) != null)
				p += .25;

		return Random.value > p;
	}

	bool IsContained(Vector2 xy)
	{
		var x = Mathf.RoundToInt(xy.x);
		var y = Mathf.RoundToInt(xy.y);
		return Mathf.Abs (x) <= GameManager.Instance.xRadius && Mathf.Abs (y) <= GameManager.Instance.yRadius;
	}

	Room CreateRoomAt(int x, int y)
	{
		var go = Instantiate (roomPrefab);
		go.transform.position = new Vector3(x, y, 0);
		go.transform.parent = transform;
		go.name = string.Format("Room ({0},{1})",x,y);

		var room = go.GetComponent<Room>();
		map.RegisterRoom(room);

		roomsCreated++;

		return room;
	}
}
