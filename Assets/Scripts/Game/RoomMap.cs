using UnityEngine;
using System.Collections;

public class RoomMap : Singleton<RoomMap> {

	public int xSize = 21;
	public int ySize = 11;

	public Room[,] rooms;

	// Use this for initialization
	void Start () {
		rooms = new Room[xSize,ySize];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RegisterRoom(Room room, bool connectToAdjacent = false)
	{
		int x = Mathf.RoundToInt(room.transform.position.x);
		int y = Mathf.RoundToInt(room.transform.position.y);
		ShiftCoordinates(ref x, ref y);

		if (rooms[x,y] != null)
			throw new UnityException("Only one room per tile is allowed.");

		rooms[x,y] = room;

		if (!connectToAdjacent)
			return;

		if (y > 0 && rooms[x,y-1] != null)
			room.Connect(rooms[x,y-1], 0);
		if (x < xSize - 1 && rooms[x+1,y] != null)
			room.Connect(rooms[x+1,y], 1);
		if (y < ySize - 1 && rooms[x,y+1] != null)
			room.Connect(rooms[x,y+1], 2);
		if (x > 0 && rooms[x-1,y] != null)
			room.Connect(rooms[x-1,y], 3);
	}

	private void ShiftCoordinates(ref int x, ref int y)
	{
		x += (xSize - 1) / 2;
		y += (ySize - 1) / 2;
	}
}
