//At the end of a level (and once at the start of the game), make sure we put all our toys away

using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class CleanupLevelCommand : Command
	{

		[Inject]
		public DestroyPlayerSignal destroyPlayerSignal{ get; set; }

		[Inject]
		public DestroyEnemySignal destroyEnemySignal{ get; set; }

		public override void Execute()
		{
			if (injectionBinder.GetBinding<GameObject> (GameElement.GAME_FIELD) != null)
			{
				GameObject gameField = injectionBinder.GetInstance<GameObject> (GameElement.GAME_FIELD);

				//Clean up the Player's ship
				if (injectionBinder.GetBinding<PlayerView> (GameElement.PLAYER_SHIP) != null)
				{
					PlayerView playerView = injectionBinder.GetInstance<PlayerView> (GameElement.PLAYER_SHIP);
					destroyPlayerSignal.Dispatch (playerView, true);
				}
				
				//Clean up rocks
				EnemyView[] enemies = gameField.GetComponentsInChildren<EnemyView> ();
				foreach (EnemyView enemy in enemies)
				{
					destroyEnemySignal.Dispatch (enemy, false);
				}
				
				//Clean up gameboard
				GameObject.Destroy(gameField);
				injectionBinder.Unbind<GameObject>(GameElement.GAME_FIELD);
			}
		}
	}
}

