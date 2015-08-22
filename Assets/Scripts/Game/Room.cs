using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviourBase {

	public GameObject doorPrefab;

	public Room[] connections = new Room[4];
	public List<Unit> inhabitants = new List<Unit>();

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

		var door = Instantiate(doorPrefab);
		door.transform.parent = transform;

		door.transform.localPosition = .5f * Unit.Dir2Vector(direction);
		door.transform.rotation = Quaternion.AngleAxis(-Unit.Dir2Deg(direction), Vector3.forward);
	}

	public void Enter(Unit unit)
	{
		if (this.inhabitants.Contains(unit))
			throw new UnityException("You invented cloning.");
		this.inhabitants.Add (unit);
	}

	public void Exit(Unit unit)
	{
		if (!this.inhabitants.Remove(unit))
			throw new UnityException("You can't leave a room you are not in!");
	}

	public bool ContainsUnit(string tag)
	{
		foreach (var u in inhabitants)
			if (u.tag == tag)
				return true;
		return false;
	}

	public bool TryGetUnit(string tag, out Unit unit)
	{
		foreach (var u in inhabitants)
			if (u.tag == tag)
		{
			unit = u;
			return true;
		}

		unit = null;
		return false;
	}
}
