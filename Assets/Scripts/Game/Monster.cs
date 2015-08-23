using UnityEngine;

public class Monster : MonoBehaviourBase {

	public TaskDefinition[] tasks;

	public float attackCooldown;
	public float currentCooldown;

	public GameObject trapPrefab;

	Unit me;

	TaskDefinition busyWith;

	public bool canAttack { get { return currentCooldown <= 0; } }

	void Start()
	{
		me = GetComponent<Unit>();
	}

	void OnNewRoomEntered(Room newRoom)
	{
		foreach (var unit in newRoom.inhabitants)
			if (unit.tag == "Adventurer")
		{
			if (!UnitWillDefend(unit, newRoom) && canAttack)
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

	bool UnitWillDefend(Unit unit, Room newRoom)
	{
		if (unit.direction == (me.direction + 2) % 4)
			return false;
		if (unit.direction == me.direction || unit.nextRoom == newRoom)
			return unit.GetComponent<Adventurer>().canDefend;
		return false;
	}

	void Update()
	{
		currentCooldown = Mathf.Max (0, currentCooldown - Time.deltaTime);

		if (me.busy)
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
		me.busy = true;
	}

	void FinishTask()
	{
		busyWith.lastUse = Time.time;

		ActionButton btn;
		if (ActionButton.allButtons.TryGetValue(busyWith.key, out btn))
			btn.StartCooldown(busyWith.cooldown);

		busyWith = null;
		me.busy = false;
	}

	void PlaceTrap()
	{
		var trap = Instantiate(trapPrefab);
		trap.transform.position = me.currentRoom.transform.position;
		trap.transform.SetParent(me.currentRoom.transform);

		FinishTask();
	}
}
