//When the user picks a direction, compute all the outcomes first


using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class EndTurnCommand : Command
	{
		[Inject]
		public string direction{ get; set; }
		
		[Inject]
		public IGameModel gameModel { get; set; }

		[Inject]
		public EndTurnSignal endTurnSignal{ get; set; }
		
		public override void Execute ()
		{
			LevelModel level = gameModel.currentLevel;
			int aa = level.enemies.Count;
			for (var a = aa - 1; a >= 0; a--) {
				ObjectStatus enemy = level.enemies[a];
				if (enemy.destroyed) {
					Debug.Log(enemy.x + ", " + enemy.y + "(" + a + "/" + level.enemies.Count + ")");

					level.enemies.RemoveAt(a);
				}
			}
			endTurnSignal.Dispatch ();
		}
	}

}

