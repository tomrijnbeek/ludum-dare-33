using UnityEngine;
using System.Collections;

public class MonsterRouter : MonoBehaviourBase, IRouter {

	public Unit me;

	public void Start()
	{
		me = GetComponent<Unit>();
	}

	public Room NextRoom()
	{
		if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && me.currentRoom.connections[0] != null)
			return me.currentRoom.connections[0];
		if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && me.currentRoom.connections[1] != null)
			return me.currentRoom.connections[1];
		if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && me.currentRoom.connections[2] != null)
			return me.currentRoom.connections[2];
		if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && me.currentRoom.connections[3] != null)
			return me.currentRoom.connections[3];

		return null;
	}
}
