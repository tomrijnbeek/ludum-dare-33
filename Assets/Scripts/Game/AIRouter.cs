using UnityEngine;
using System.Collections;
using System.Linq;

public class AIRouter : MonoBehaviourBase, IRouter {

	public Unit me;
	public int lastDir;
	
	public void Start()
	{
		me = GetComponent<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int? NextDir()
	{
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
