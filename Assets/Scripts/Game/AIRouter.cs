using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIRouter : MonoBehaviourBase, IRouter {

	public Unit me;
	public int lastDir;

	public int viewRange;

	public bool inChase;
	public Room lastKnownMonsterPosition;
	public float timeSinceLastMonsterObservation;

	public float slowMoveSpeed;
	public float fastMoveSpeed;

	public float chaseCooldownTime;
	
	public void Start()
	{
		me = GetComponent<Unit>();
		me.moveSpeed = slowMoveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastMonsterObservation += Time.deltaTime;

		if (inChase && timeSinceLastMonsterObservation >= chaseCooldownTime)
		{
			inChase = false;
			lastKnownMonsterPosition = null;
			me.moveSpeed = slowMoveSpeed;
		}

		LookInDirection(me.direction);
	}

	void LookInDirection(int direction)
	{
		var room = me.currentRoom;
		
		for (int i = 0; i < viewRange; i++)
		{
			if (room.connections[direction] == null)
				break;
			room = room.connections[direction];
			
			if (room.ContainsUnit("Player"))
			{
				inChase = true;
				lastKnownMonsterPosition = room;
				timeSinceLastMonsterObservation = 0;
				me.moveSpeed = fastMoveSpeed;
			}
		}
		
		Debug.DrawLine(transform.position, room.transform.position, Color.magenta);
	}

	public int DirToMonster()
	{
		var dict = new Dictionary<Room, int>();
		var q = new Queue<Room>();
		var hs = new HashSet<Room>();

		var start = me.nextRoom ?? me.currentRoom;

		if (start == lastKnownMonsterPosition)
			return me.direction;

		for (int i = 0; i < 4; i++)
			if (start.connections[i] != null)
			{
				if (start.connections[i] == lastKnownMonsterPosition)
					return i;
				dict.Add(start.connections[i], i);
				q.Enqueue(start.connections[i]);
				hs.Add(start.connections[i]);
			}

		while (q.Count > 0)
		{
			var room = q.Dequeue();
			var dir = dict[room];

			for (int i = 0; i < 4; i++)
				if (room.connections[i] != null && !hs.Contains(room.connections[i]))
				{
					if (room.connections[i] == lastKnownMonsterPosition)
						return dir;
					dict.Add(room.connections[i], dir);
					q.Enqueue(room.connections[i]);
					hs.Add(room.connections[i]);
				}
		}

		throw new UnityException("No path to monster found.");
	}

	public int? NextDir()
	{
		if (inChase)
		{
			// We arrived in the room where we last saw the monster of we lost it earlier during the chase, so we look around.
			if (lastKnownMonsterPosition == null || me.currentRoom == lastKnownMonsterPosition)
			{
				for (int i = 0; i < 4; i++)
					LookInDirection(i);
			}

			// Didn't find a new monster position except for the current?
			if (me.currentRoom == lastKnownMonsterPosition)
				lastKnownMonsterPosition = null;

			// We know where the monster is. Chase it!
			if (lastKnownMonsterPosition != null)
				return DirToMonster();

			// Fall back on old code if we don't know for sure in which direction monster went.
		}

		// Check for dead ends first
		if (me.currentRoom.connections.Count (r => r != null) == 1)
			return (lastDir + 2) % 4;
		
		int dir = -1;

		while (dir == -1 || me.currentRoom.connections[dir] == null || dir == (lastDir + 2) % 4)
			// this will fail for disconnected rooms
			dir = Random.Range(0, me.currentRoom.connections.Length);

		lastDir = dir;
		
		return dir;
	}
}
