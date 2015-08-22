using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviourBase {

	public GameObject trapPrefab;

	Unit me;

	void Start()
	{
		me = GetComponent<Unit>();
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
		if (me.nextRoom != null)
		{
			int? dir;
			if (me.router != null && (dir = me.router.NextDir()) != null && dir.Value == (me.direction + 2) % 4)
				me.TurnAround();
		}
		else
		{
			if (Input.GetKey(KeyCode.Space) && !me.currentRoom.ContainsUnit("Trap"))
				SetTrap ();
		}
	}

	void SetTrap()
	{
		var trap = Instantiate(trapPrefab);
		trap.transform.position = me.currentRoom.transform.position;
		trap.transform.SetParent(me.currentRoom.transform);
	}
}
