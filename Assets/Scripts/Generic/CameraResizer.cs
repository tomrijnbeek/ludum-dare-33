using UnityEngine;
using System.Collections;

public class CameraResizer : MonoBehaviour {

	float defaultSize;

	public float aspectRatio = 16.0f / 9.0f;

	void Awake() {
		defaultSize = Camera.main.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
		var expectedWidth = aspectRatio * Screen.height;

		if (Screen.width < expectedWidth)
			Camera.main.orthographicSize = defaultSize * (expectedWidth / Screen.width);
		else
			Camera.main.orthographicSize = defaultSize;

	}
}
