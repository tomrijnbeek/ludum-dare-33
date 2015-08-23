using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActionButton : MonoBehaviour {

	public static Dictionary<KeyCode, ActionButton> allButtons;

	public KeyCode key;
	public Text text;
	public Image circle;

	public float totalCooldown, cooldownLeft;

	void Start()
	{
		if (allButtons == null)
			allButtons = new Dictionary<KeyCode, ActionButton>();
		allButtons.Add (key, this);
	}

	public void StartCooldown(float time)
	{
		if (time <= 0)
			return;

		totalCooldown = time;
		cooldownLeft = time;

		text.gameObject.SetActive(false);
		circle.gameObject.SetActive(true);
		circle.fillAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (totalCooldown <= 0)
			return;

		cooldownLeft -= Time.deltaTime;

		if (cooldownLeft <= 0)
		{
			totalCooldown = 0;
			cooldownLeft = 0;

			text.gameObject.SetActive(true);
			circle.gameObject.SetActive(false);

			return;
		}

		circle.fillAmount = 1 - (cooldownLeft / totalCooldown);
	}
}
