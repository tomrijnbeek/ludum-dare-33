using UnityEngine;
using System.Collections;

public class RoomMap : Singleton<RoomMap> {

	public int xSize = 21;
	public int ySize = 11;

	public Room[,] rooms;

	void validateRoomExistance()
	{
		if (rooms == null)
			rooms = new Room[xSize,ySize];
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void GetCoordinatesAt(Vector3 position, out int x, out int y)
	{
		x = Mathf.RoundToInt(position.x);
		y = Mathf.RoundToInt(position.y);
		ShiftCoordinates(ref x, ref y);
	}

	public Room GetRoomAt(Vector3 position)
	{
		validateRoomExistance();

		int x, y;
		GetCoordinatesAt(position, out x, out y);
		return rooms[x, y];
	}

	public void RegisterRoom(Room room, bool connectToAdjacent = false)
	{
		validateRoomExistance();

		int x, y;
		GetCoordinatesAt(room.transform.position, out x, out y);

		if (rooms[x,y] != null)
			throw new UnityException("Only one room per tile is allowed.");

		rooms[x,y] = room;

		if (!connectToAdjacent)
			return;

		if (y < ySize - 1 && rooms[x,y+1] != null)
			room.Connect(rooms[x,y+1], 0);
		if (x < xSize - 1 && rooms[x+1,y] != null)
			room.Connect(rooms[x+1,y], 1);
		if (y > 0 && rooms[x,y-1] != null)
			room.Connect(rooms[x,y-1], 2);
		if (x > 0 && rooms[x-1,y] != null)
			room.Connect(rooms[x-1,y], 3);
	}

	public void ShiftCoordinates(ref int x, ref int y)
	{
		x += (xSize - 1) / 2;
		y += (ySize - 1) / 2;
	}
}
