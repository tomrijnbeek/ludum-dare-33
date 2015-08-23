using UnityEngine;
using System.Collections;

public class Archer : Adventurer {

	public override void MonsterDetected (Unit monster, Room room)
	{
		var oldCanDefend = canDefend;
		canDefend |= room != me.currentRoom;

		BattleMonster(monster);

		canDefend = oldCanDefend;
	}
}
