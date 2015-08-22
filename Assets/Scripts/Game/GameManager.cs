using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	public bool gameOver;

	public Unit player;

	public void GameOver() {
		gameOver = true;

		Destroy (player.gameObject);

		Debug.Log("You lost");
	}
}
