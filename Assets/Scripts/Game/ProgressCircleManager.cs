using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProgressCircleManager : Singleton<ProgressCircleManager> {

	public GameObject prefab;

	int counter;
	Dictionary<int, Image> circles = new Dictionary<int, Image>();

	public int MakeCircle(Vector3 worldPosition, Color? color = null)
	{
		var go = Instantiate(prefab);
		go.transform.SetParent(this.transform);

		var rt = go.GetComponent<RectTransform>();
		rt.position = Camera.main.WorldToScreenPoint(worldPosition);

		var image = go.GetComponent<Image>();
		circles.Add(counter, image);
		if (color != null)
			image.color = color.Value;

		return counter++;
	}

	public void MoveCircle(int handle, Vector3 worldPosition)
	{
		circles[handle].GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(worldPosition);
	}

	public void ColorCircle(int handle, Color color)
	{
		circles[handle].color = color;
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
