using UnityEngine;
using System.Collections;

public class AIRouter : MonoBehaviourBase, IRouter {

	public Unit me;
	
	public void Start()
	{
		me = GetComponent<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Room NextRoom()
	{
		Room r = null;

		while (r == null)
			// this will fail for disconnected rooms
			r = me.currentRoom.connections[Random.Range(0, me.currentRoom.connections.Length)];
		
		return r;
	}
}
