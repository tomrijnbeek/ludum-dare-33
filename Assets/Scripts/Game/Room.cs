using UnityEngine;
using System.Collections;

public class Room : MonoBehaviourBase {

	public Room[] connections = new Room[4];
	RoomMap map;

	// Use this for initialization
	void Start () {
		map = RoomMap.Instance;
		map.RegisterRoom(this);
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var conn in connections)
			if (conn != null && conn.GetInstanceID() < this.GetInstanceID())
				Debug.DrawLine(transform.position, conn.transform.position, Color.yellow);
	}

	public void Connect(Room room, int direction)
	{
		var invertDir = (direction + 2) % 4;
		if (this.connections[direction] != null || room.connections[invertDir] != null)
			throw new UnityException("Can't replace existing connections.");
		this.connections[direction] = room;
		room.connections[invertDir] = this;
	}
}
