using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class ExpandAndFade : MonoBehaviour {

	public float startScale, finalScale, scaleSpeed;
	public float currentScale;

	float startAlpha;
	SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();

		startAlpha = sprite.color.a;

		currentScale = startScale;
		transform.localScale = currentScale * Vector3.one;
	}
	
	// Update is called once per frame
	void Update () {
		var step = scaleSpeed * Time.deltaTime;

		if (Mathf.Abs (currentScale - finalScale) < step)
		{
			Destroy(this.gameObject);
			return;
		}

		currentScale += step;

		transform.localScale = currentScale * Vector3.one;
		sprite.color = sprite.color.WithAlpha(startAlpha * (1 - (currentScale - startScale) / (finalScale - startScale)));
	}
}
