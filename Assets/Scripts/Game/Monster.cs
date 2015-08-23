using UnityEngine;

public class Monster : MonoBehaviourBase {

	public TaskDefinition[] tasks;

	public float attackCooldown;
	public float currentCooldown;

	public GameObject trapPrefab;

	Unit me;

	public bool busy;
	TaskDefinition busyWith;

	void Start()
	{
		me = GetComponent<Unit>();
	}

	void OnNewRoomEntered(Room newRoom)
	{
		foreach (var unit in newRoom.inhabitants)
			if (unit.tag == "Adventurer")
		{
			if (unit.direction != (me.direction + 2) % 4 && (unit.direction == me.direction || unit.nextRoom != newRoom) && currentCooldown <= 0)
			{
				unit.SendMessage("Kill");
				currentCooldown = attackCooldown;

				ActionButton btn;
				if (ActionButton.allButtons.TryGetValue(KeyCode.None, out btn))
					btn.StartCooldown(attackCooldown);
			}
			else
				GameManager.Instance.GameOver();
		}
	}

	void Update()
	{
		currentCooldown = Mathf.Max (0, currentCooldown - Time.deltaTime);

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
			for (int i = 0; i < tasks.Length; i++)
				if (Input.GetKey(tasks[i].key) && Time.time - tasks[i].lastUse >= tasks[i].cooldown)
					StartTask(tasks[i]);
		}
	}

	void StartTask(TaskDefinition task)
	{
		var go = Instantiate(task.prefab);
		go.transform.position = transform.position;
		go.transform.SetParent(this.transform);

		busyWith = task;
		busy = true;
	}

	void FinishTask()
	{
		busyWith.lastUse = Time.time;

		ActionButton btn;
		if (ActionButton.allButtons.TryGetValue(busyWith.key, out btn))
			btn.StartCooldown(busyWith.cooldown);

		busyWith = null;
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
