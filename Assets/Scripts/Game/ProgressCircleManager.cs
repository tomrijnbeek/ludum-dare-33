using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProgressCircleManager : Singleton<ProgressCircleManager> {

	public GameObject prefab;

	int counter;
	Dictionary<int, Image> circles = new Dictionary<int, Image>();

	public int MakeCircle(Vector3 worldPosition)
	{
		var go = Instantiate(prefab);
		go.transform.SetParent(this.transform);

		var rt = go.GetComponent<RectTransform>();
		rt.position = Camera.main.WorldToScreenPoint(worldPosition);
		circles.Add(counter, go.GetComponent<Image>());

		return counter++;
	}

	public void UpdateCircle(int handle, float t)
	{
		var img = circles[handle];
		img.fillAmount = t;
	}

	public void DeleteCircle(int handle)
	{
		var img = circles[handle];
		circles.Remove(handle);
		Destroy(img.gameObject);
	}
}
