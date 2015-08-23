using UnityEngine;
using System.Collections;

public class Invisibility : MonoBehaviourBase {

	public int progressBarHandle;

	public float lifeTime, timeLeft;
	
	// Use this for initialization
	void Start () {
		timeLeft = lifeTime;

		progressBarHandle = ProgressCircleManager.Instance.MakeCircle(transform.position);
		ProgressCircleManager.Instance.UpdateCircle(progressBarHandle, 1);

		var sr = GetComponentInChildren<SpriteRenderer>();
		sr.color = sr.color.WithAlpha(0.5f);
	}
	
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;

		ProgressCircleManager.Instance.UpdateCircle(progressBarHandle, timeLeft / lifeTime);
		ProgressCircleManager.Instance.MoveCircle(progressBarHandle, transform.position);

		if (timeLeft <= 0)
		{
			var sr = GetComponentInChildren<SpriteRenderer>();
			sr.color = sr.color.WithAlpha(1);
			ProgressCircleManager.Instance.DeleteCircle(progressBarHandle);
			Destroy (this);
		}
	}

}
