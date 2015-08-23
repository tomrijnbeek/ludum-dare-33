using UnityEngine;
using System.Collections;

public class Rogue : Adventurer {
	public float trapDisarmTime;

	protected override void OnTrapEncountered (Unit trapUnit)
	{
		SetBusyFor(trapDisarmTime, true, () => { Destroy(trapUnit.gameObject); });
	}

	protected override void ExtraUpdate ()
	{
		for (int i = 0; i < 4; i++)
			LookInDirection(i, 1);
	}
}
