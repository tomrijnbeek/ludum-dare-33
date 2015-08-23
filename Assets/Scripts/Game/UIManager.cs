using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : Singleton<UIManager> {

	public bool tutorialEnabled;
	public Canvas tutorialCanvas, gameOverCanvas;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;

		if (!tutorialEnabled)
			DisableTutorialCanvas();
	}
	
	// Update is called once per frame
	void Update () {
		if (tutorialEnabled && Input.anyKeyDown)
			DisableTutorialCanvas();
	}

	public void DisableTutorialCanvas()
	{
		Destroy (tutorialCanvas.gameObject);
		Time.timeScale = 1;
		tutorialEnabled = false;
	}

	public void EnableGameOverCanvas()
	{
		gameOverCanvas.gameObject.SetActive(true);
	}
}
