using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterTask : MonoBehaviourBase {

	public string taskName;
	public float time;

	public int progressHandle;

	public float timeLeft;

	// Use this for initialization
	void Start () {
		timeLeft = time;
		progressHandle = ProgressCircleManager.Instance.MakeCircle(this.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;

		ProgressCircleManager.Instance.UpdateCircle(progressHandle, 1 - (timeLeft / time));

		if (timeLeft <= 0)
		{
			TaskDone();
		}
	}

	void TaskDone() {
		SendMessageUpwards(taskName);
		ProgressCircleManager.Instance.DeleteCircle(progressHandle);
		Destroy (this.gameObject);
	}
}
