//At the end of a level (and once at the start of the game), make sure we put all our toys away

using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class CleanupLevelCommand : Command
	{
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		[Inject]
		public DestroyPlayerSignal destroyPlayerSignal{ get; set; }

		[Inject]
		public DestroyEnemySignal destroyEnemySignal{ get; set; }

		public override void Execute()
		{
			//Clean up the Player's ship
			if (injectionBinder.GetBinding<ShipView> (GameElement.PLAYER_SHIP) != null)
			{
				ShipView shipView = injectionBinder.GetInstance<ShipView> (GameElement.PLAYER_SHIP);
				destroyPlayerSignal.Dispatch (shipView, true);
			}

			//Clean up rocks
			EnemyView[] rocks = gameField.GetComponentsInChildren<EnemyView> ();
			foreach (EnemyView rock in rocks)
			{
				destroyEnemySignal.Dispatch (rock, false);
			}
		}
	}
}

