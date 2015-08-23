using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	
	public bool gameOver;

	public Unit player;
	
	public int adventurersQueued;
	public float nextAdventurerSpawn, nextChestSpawn;
	public GameObject[] adventurerPrefabs;
	public GameObject chestPrefab;
	public int[] adventurerPrefabThresholds;

	public float timeBetweenAdventurers;
	public float timeBetweenChests;
	public int xRadius = 7;
	public int yRadius = 4;

	public int kills;
	public Text killsLabel;

	public void Awake()
	{
		if (adventurersQueued > 0)
			SpawnAdventurer();
		
		if (adventurersQueued > 0)
			nextAdventurerSpawn = timeBetweenAdventurers;

		nextChestSpawn = .5f * timeBetweenChests;
	}

	public void GameOver() {
		gameOver = true;
		Time.timeScale = 0;

		Destroy (player.gameObject);
		UIManager.Instance.EnableGameOverCanvas();
	}

	void Update()
	{
		if (gameOver)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
			else if (Input.anyKeyDown)
			{
				if (ActionButton.allButtons != null)
					ActionButton.allButtons.Clear();
				if (Adventurer.all != null)
					Adventurer.all.Clear();
				Application.LoadLevel(Application.loadedLevel);
			}

			return;
		}

		nextAdventurerSpawn = Mathf.Max (0, nextAdventurerSpawn - Time.deltaTime);
		nextChestSpawn = Mathf.Max (0, nextChestSpawn - Time.deltaTime);

		if (adventurersQueued > 0 && nextAdventurerSpawn <= 0)
		{
			for (int i = 0, n = adventurersQueued / 5 + 1; i < n; i++)
				SpawnAdventurer();
			nextAdventurerSpawn = timeBetweenAdventurers;
		}

		if (nextChestSpawn <= 0)
		{
			SpawnChest();
			nextChestSpawn = timeBetweenChests;
		}
	}

	public void AddKill()
	{
		kills++;
		killsLabel.text = kills.ToString();
	}

	public void QueueAdventurer()
	{
		if (adventurersQueued == 0)
			nextAdventurerSpawn = timeBetweenAdventurers;
		adventurersQueued++;
	}

	void SpawnAdventurer()
	{
		const int minSpawnDistance = 5;

		SpawnSomething(SelectAdventurer(), minSpawnDistance);

		adventurersQueued--;
	}

	void SpawnChest()
	{
		const int minSpawnDistance = 4;

		SpawnSomething(chestPrefab, minSpawnDistance);
	}

	void SpawnSomething(GameObject prefab, int minSpawnDistance)
	{
		var map = RoomMap.Instance;
		
		int x;
		int y;
		
		int playerX = player.currentRoom != null ? Mathf.RoundToInt(player.currentRoom.transform.position.x) : 0;
		int playerY = player.currentRoom != null ? Mathf.RoundToInt(player.currentRoom.transform.position.y) : 0;
		
		do
		{
			x = Random.Range(-xRadius, xRadius);
			y = Random.Range(-yRadius, yRadius);
		} while (map.GetRoomAt(new Vector3(x,y,0)) == null
		         || Mathf.Abs(playerX - x) + Mathf.Abs(playerY - y) < minSpawnDistance
		         || map.GetRoomAt(new Vector3(x,y,0)).inhabitants.Count > 0);
		
		var adv = Instantiate(prefab);
		adv.transform.position = new Vector3(x,y,0);
	}

	GameObject SelectAdventurer()
	{
		var possibilities = new List<int>();

		for (int i = 0; i < adventurerPrefabs.Length; i++)
		{
			if (adventurerPrefabThresholds[i] <= kills)
				possibilities.Add (i);
		}

		return adventurerPrefabs[possibilities[Random.Range(0, possibilities.Count)]];
	}
}
