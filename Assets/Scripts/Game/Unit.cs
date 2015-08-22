using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviourBase {

	public Room currentRoom;
	public Room nextRoom;

	public float moveSpeed;
	public IRouter router;

	// Use this for initialization
	void Start () {
		currentRoom = RoomMap.Instance.GetRoomAt(this.transform.position);
		this.transform.position = currentRoom.transform.position;
		router = this.GetComponent<IRouter>();
	}
	
	// Update is called once per frame
	void Update () {
		if (nextRoom == null && router != null)
			nextRoom = router.NextRoom();
		if (nextRoom != null)
		{
			var diff = nextRoom.transform.position - this.transform.position;
			if (nextRoom != currentRoom && (currentRoom.transform.position - this.transform.position).sqrMagnitude > diff.sqrMagnitude)
				currentRoom = nextRoom;

			var s = moveSpeed * Time.deltaTime;
			if (diff.sqrMagnitude <= s * s)
			{
				this.transform.position = nextRoom.transform.position;
				nextRoom = null;
			}
			else
			{
				this.transform.position += s * diff.normalized;
			}
		}
	}
}
