using UnityEngine;
using System.Collections;

public class Room : MonoBehaviourBase {

	public Room[] connections = new Room[4];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
