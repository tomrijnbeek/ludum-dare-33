using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	public bool gameOver;

	public Unit player;

	public int numAdventurers;
	public GameObject adventurerPrefab;

	public void GameOver() {
		gameOver = true;

		Destroy (player.gameObject);

		Debug.Log("You lost");
	}

	void Awake()
	{
		for (int i = 0; i < numAdventurers; i++)
			SpawnAdventurer();
	}

	void Update()
	{

	}

	void SpawnAdventurer()
	{
		const int minSpawnDistance = 5;

		var map = RoomMap.Instance;
		var gen = GetComponent<DungeonGenerator>();

		int x;
		int y;

		int playerX = player.currentRoom != null ? Mathf.RoundToInt(player.currentRoom.transform.position.x) : 0;
		int playerY = player.currentRoom != null ? Mathf.RoundToInt(player.currentRoom.transform.position.y) : 0;

		do
		{
			x = Random.Range(-gen.xRadius, gen.xRadius);
			y = Random.Range(-gen.yRadius, gen.yRadius);
		} while (map.GetRoomAt(new Vector3(x,y,0)) == null
		         || Mathf.Abs(playerX - x) + Mathf.Abs(playerY - y) < minSpawnDistance
		         || map.GetRoomAt(new Vector3(x,y,0)).inhabitants.Count > 0);

		var adv = Instantiate(adventurerPrefab);
		adv.transform.position = new Vector3(x,y,0);
	}
}
