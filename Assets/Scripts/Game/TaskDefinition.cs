using UnityEngine;
using System.Collections;

[System.Serializable]
public class TaskDefinition
{
	public GameObject prefab;
	public KeyCode key;
	public float cooldown;
	public float lastUse = -float.MaxValue;
}
