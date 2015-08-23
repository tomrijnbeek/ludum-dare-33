using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour {

	public float scaleTime, lifeTime, wigglePeriod;
	public float t;

	Vector3 goalScale;
	Vector3 goalPosition;

	// Use this for initialization
	void Start () {
		goalPosition = transform.localPosition;
		goalScale = transform.localScale;

		transform.localScale = Vector3.zero;
		transform.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;

		if (t >= lifeTime)
		{
			Destroy (gameObject);
			return;
		}

		var f = Mathf.Min (1, t / scaleTime);

		transform.localScale = f * goalScale;
		transform.localPosition = f * goalPosition;

		if (wigglePeriod > 0)
			transform.rotation = Quaternion.AngleAxis(15 * Mathf.Sin (Mathf.PI * 2 * t / wigglePeriod), Vector3.forward);
	}
}
