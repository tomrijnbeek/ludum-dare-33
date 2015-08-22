using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviourBase {

	public Room currentRoom;
	public Room nextRoom;
	public int direction;

	public float moveSpeed;
	public IRouter router;

	// Use this for initialization
	void Start () {
		currentRoom = RoomMap.Instance.GetRoomAt(this.transform.position);
		currentRoom.Enter(this);
		this.transform.position = currentRoom.transform.position;
		router = this.GetComponent<IRouter>();
	}
	
	// Update is called once per frame
	void Update () {
		if (nextRoom == null && router != null)
		{
			var newDir = router.NextDir();
			direction = newDir ?? direction;

			if (newDir == null)
			{
				nextRoom = null;
				return;
			}

			nextRoom = currentRoom.connections[direction];
			if (nextRoom == null)
				throw new UnityException("There is no room there!");
		}
		if (nextRoom != null)
		{
			var diff = nextRoom.transform.position - this.transform.position;
			if (nextRoom != currentRoom && (currentRoom.transform.position - this.transform.position).sqrMagnitude > diff.sqrMagnitude)
			{
				currentRoom.Exit (this);
				currentRoom = nextRoom;
				currentRoom.Enter(this);
				this.SendMessage("OnNewRoomEntered", nextRoom, SendMessageOptions.DontRequireReceiver);
			}

			var s = moveSpeed * Time.deltaTime;
			if (diff.sqrMagnitude <= s * s)
			{
				this.transform.position = nextRoom.transform.position;
				nextRoom = null;
			}
			else
			{
				this.transform.position += s * Dir2Vector(direction);
			}
		}
	}

	public void TurnAround()
	{
		direction = (direction + 2) % 4;
		nextRoom = nextRoom.connections[direction];
	}

	void OnDestroy()
	{
		if (currentRoom != null)
			currentRoom.Exit (this);
	}

	public static Vector3 Dir2Vector(int dir)
	{
		switch (dir)
		{
		case 0:
			return new Vector3(0,1);
		case 1:
			return new Vector3(1,0);
		case 2:
			return new Vector3(0,-1);
		case 3:
			return new Vector3(-1,0);
		default:
			throw new System.ArgumentOutOfRangeException();
		}
	}
}
