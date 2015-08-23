using UnityEngine;
using System.Collections;

public class Decay : MonoBehaviourBase {

	public int progressBarHandle;

	public float lifeTime, timeLeft;

	// Use this for initialization
	void Start () {
		timeLeft = lifeTime;

		progressBarHandle = ProgressCircleManager.Instance.MakeCircle(transform.position);
		ProgressCircleManager.Instance.UpdateCircle(progressBarHandle, 1);
	}

	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;

		ProgressCircleManager.Instance.UpdateCircle(progressBarHandle, timeLeft / lifeTime);

		if (timeLeft <= 0)
		{
			Destroy (this.gameObject);
		}
	}

	void OnDestroy() {
		ProgressCircleManager.Instance.DeleteCircle(progressBarHandle);
	}
}
