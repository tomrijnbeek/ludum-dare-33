using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : Singleton<UIManager> {

	public Canvas tutorialCanvas;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisableTutorialCanvas()
	{
		Destroy (tutorialCanvas.gameObject);
		Time.timeScale = 1;
	}
}
