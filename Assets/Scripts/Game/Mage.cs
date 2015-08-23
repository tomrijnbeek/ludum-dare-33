using UnityEngine;
using System.Collections;

public class Mage : Adventurer {

	public float notifyRange;

	public override void MonsterDetected (Unit monster, Room room)
	{
		foreach (var adventurer in Adventurer.all)
		{
			if (adventurer != this && (adventurer.transform.position - transform.position).sqrMagnitude <= notifyRange * notifyRange)
				adventurer.MonsterDetected(monster, room);
		}
	}
}
