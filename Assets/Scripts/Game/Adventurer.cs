using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Adventurer : MonoBehaviourBase, IRouter {

	public static List<Adventurer> all;

	public bool kill;

	public Unit me;

	public int lastDir;
	
	public int viewRange;
	
	public bool inChase;
	public Room lastKnownMonsterPosition;
	public int lastKnownMonsterDirection;
	public float timeSinceLastMonsterObservation;
	
	public float slowMoveSpeed;
	public float fastMoveSpeed;
	
	public float chaseCooldownTime;

	public float busyFor, totalBusyFor;
	public int progressCircleHandle;
	System.Action busyCallback;

	public bool canDefend = true;

	bool[] directionsLooked;

	void OnNewRoomEntered(Room newRoom)
	{
		Unit u;
		if (newRoom.TryGetUnit("Player", out u))
		{
			if (canDefend || !u.GetComponent<Monster>().canAttack)
				GameManager.Instance.GameOver();
			else
			{
				Kill ();
				return;
			}
		}

		Unit trapUnit;
		if (newRoom.TryGetUnit("Trap", out trapUnit))
		{
			OnTrapEncountered(trapUnit);
		}
	}

	protected void SetBusyFor(float time, bool showProgressCircle = false, System.Action callback = null)
	{
		me.busy = true;
		totalBusyFor = time;
		busyFor = time;

		progressCircleHandle = showProgressCircle ? ProgressCircleManager.Instance.MakeCircle(transform.position) : 0;

		busyCallback = callback;
	}

	protected virtual void OnTrapEncountered(Unit trapUnit)
	{
		Kill ();
		Destroy (trapUnit.gameObject);
	}

	void Update()
	{
		if (kill)
			Kill();

		timeSinceLastMonsterObservation += Time.deltaTime;

		if (me.busy)
		{
			busyFor -= Time.deltaTime;
			if (busyFor > 0)
			{
				if (progressCircleHandle >= 0)
					ProgressCircleManager.Instance.UpdateCircle(progressCircleHandle, 1 - (busyFor / totalBusyFor));
				return;
			}
			me.busy = false;
			if (progressCircleHandle >= 0)
				ProgressCircleManager.Instance.DeleteCircle(progressCircleHandle);
			if (busyCallback != null)
				busyCallback();
		}

		ExtraUpdate();

		if (inChase && timeSinceLastMonsterObservation >= chaseCooldownTime)
		{
			inChase = false;
			lastKnownMonsterPosition = null;
			lastKnownMonsterDirection = -1;
			me.moveSpeed = slowMoveSpeed;
		}
		
		LookInDirection(me.direction);
	}

	protected virtual void ExtraUpdate() { }

	void Kill()
	{
		Destroy (this.gameObject);
		GameManager.Instance.AddKill();
		GameManager.Instance.QueueAdventurer();
		GameManager.Instance.QueueAdventurer();
	}
	
	public void Start()
	{
		if (all == null)
			all = new List<Adventurer>();
		all.Add (this);

		me = GetComponent<Unit>();
		me.moveSpeed = slowMoveSpeed;
	}

	protected void LookInDirection(int direction)
	{
		LookInDirection(direction, viewRange);
	}
	
	protected void LookInDirection(int direction, int range)
	{
		var room = me.currentRoom;
		
		for (int i = 0; i < range; i++)
		{
			if (room.connections[direction] == null)
				break;
			room = room.connections[direction];
			
			Unit monster;
			
			if (room.TryGetUnit("Player", out monster))
				MonsterDetected(monster, room);
		}
		
		Debug.DrawLine(transform.position, room.transform.position, Color.magenta);
	}

	public virtual void MonsterDetected(Unit monster, Room room)
	{
		directionsLooked = null;

		inChase = true;
		lastKnownMonsterPosition = room;
		lastKnownMonsterDirection = monster.direction;
		timeSinceLastMonsterObservation = 0;
		me.moveSpeed = fastMoveSpeed;

		if (DirToMonster() == (me.direction + 2) % 4)
		{
			me.TurnAround();
			lastDir = me.direction;
		}
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
			// We arrived in the room where we last saw the monster or we lost it earlier during the chase, so we look around.
			if (lastKnownMonsterPosition == null || me.currentRoom == lastKnownMonsterPosition)
			{
				if (directionsLooked == null)
				{
					directionsLooked = new bool[4];
					directionsLooked[lastDir] = true;
					directionsLooked[(lastDir + 1) % 4] = me.currentRoom.connections[(lastDir + 1) % 4] == null;
					directionsLooked[(lastDir + 2) % 4] = true;
					directionsLooked[(lastDir + 3) % 4] = me.currentRoom.connections[(lastDir + 3) % 4] == null;
				}

				for (int i = 0; i < 4; i++)
					if (!directionsLooked[i])
				{
					directionsLooked[i] = true;
					return 4 + i;
				}
			}
			
			// Didn't find a new monster position?
			if (me.currentRoom == lastKnownMonsterPosition)
			{
				lastKnownMonsterPosition = null;
				
				// Well, we may still know in which direction it went.
				if (lastKnownMonsterDirection >= 0)
				{
					var spottedDir = lastKnownMonsterDirection;
					lastKnownMonsterDirection = -1;
					return spottedDir;
				}
			}
			
			// We know where the monster is. Chase it!
			if (lastKnownMonsterPosition != null)
			{
				var monsterDir = DirToMonster();
				lastDir = monsterDir;
				return monsterDir;
			}
			
			// Fall back on old code if we don't know for sure in which direction monster went.
		}

		var count = me.currentRoom.connections.Count (r => r != null);
		
		// Check for dead ends first
		if (count == 1)
		{
			for (int i = 0; i < 4; i++)
				if (me.currentRoom.connections[i] != null)
			{
				lastDir = i;
				return i;
			}
		}
		
		int dir = -1;
		int tries = 0;
		
		while (dir == -1
		       || me.currentRoom.connections[dir] == null
		       || dir == (lastDir + 2) % 4
		       || (!canDefend && tries < 10 && count > 2 && me.currentRoom.connections[dir].ContainsUnit("Player")))
		{
			// this will fail for disconnected rooms
			dir = Random.Range(0, me.currentRoom.connections.Length);
			tries++;
		}
		
		lastDir = dir;
		
		return dir;
	}

	void OnDestroy()
	{
		all.Remove (this);
	}
}
