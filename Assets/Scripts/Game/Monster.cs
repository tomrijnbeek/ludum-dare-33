﻿using UnityEngine;

public class Monster : MonoBehaviourBase {

	public GameObject[] taskPrefabs;
	public KeyCode[] taskKeys;

	public GameObject trapPrefab;

	Unit me;

	public bool busy;

	void Start()
	{
		me = GetComponent<Unit>();

		if (taskPrefabs.Length != taskKeys.Length)
			Debug.LogWarning("Number of task prefabs and key bindings do not match.");
	}

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

	void Update()
	{
		if (busy)
			return;

		if (me.nextRoom != null)
		{
			int? dir;
			if (me.router != null && (dir = me.router.NextDir()) != null && dir.Value == (me.direction + 2) % 4)
				me.TurnAround();
		}
		else if (!me.currentRoom.ContainsUnit("Trap"))
		{
			for (int i = 0; i < taskKeys.Length; i++)
				if (Input.GetKey(taskKeys[i]))
					StartTask(i);
		}
	}

	void StartTask(int i)
	{
		var task = Instantiate(taskPrefabs[i]);
		task.transform.position = transform.position;
		task.transform.SetParent(this.transform);
		busy = true;
	}

	void FinishTask()
	{
		busy = false;
	}

	void PlaceTrap()
	{
		var trap = Instantiate(trapPrefab);
		trap.transform.position = me.currentRoom.transform.position;
		trap.transform.SetParent(me.currentRoom.transform);

		FinishTask();
	}
}
