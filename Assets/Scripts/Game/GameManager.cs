﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : Singleton<GameManager> {
	
	public bool gameOver;

	public Unit player;
	
	public int adventurersQueued;
	public float nextSpawnMoment;
	public GameObject adventurerPrefab;

	public float timeBetweenSpawns = 10;
	public int xRadius = 7;
	public int yRadius = 4;

	public int kills;
	public Text killsLabel;

	public void Awake()
	{
		if (adventurersQueued > 0)
			SpawnAdventurer();
		
		if (adventurersQueued > 0)
			nextSpawnMoment = timeBetweenSpawns;
	}

	public void GameOver() {
		gameOver = true;

		Destroy (player.gameObject);

		Debug.Log("You lost");
	}

	void Update()
	{
		nextSpawnMoment = Mathf.Max (0, nextSpawnMoment - Time.deltaTime);
		if (adventurersQueued > 0 && nextSpawnMoment <= 0)
		{
			for (int i = 0, n = adventurersQueued / 5 + 1; i < n; i++)
				SpawnAdventurer();
			nextSpawnMoment = timeBetweenSpawns;
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
			nextSpawnMoment = timeBetweenSpawns;
		adventurersQueued++;
	}

	void SpawnAdventurer()
	{
		const int minSpawnDistance = 5;

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

		var adv = Instantiate(adventurerPrefab);
		adv.transform.position = new Vector3(x,y,0);

		adventurersQueued--;
	}
}
