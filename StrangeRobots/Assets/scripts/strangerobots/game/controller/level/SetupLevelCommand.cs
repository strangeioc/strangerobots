//At the start of each level, place the player and the rocks

using System;
using System.Collections;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.context.api;

namespace strange.examples.strangerobots.game
{
	public class SetupLevelCommand : Command
	{
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public CreatePlayerSignal createPlayerSignal{ get; set; }

		[Inject]
		public CreateEnemySignal createEnemySignal{ get; set; }

		[Inject]
		public IGameConfig gameConfig{ get; set; }

		public override void Execute ()
		{
			ILevelConfig levelConfig = gameConfig.getLevel (gameModel.level);
			gameModel.currentLevel = new LevelModel (levelConfig);

			var halfW = (float)gameModel.currentLevel.width * .5f;
			var halfH = (float)gameModel.currentLevel.height * .5f;

			int enemyCount = gameModel.currentLevel.enemies.Count;
			for (int a = 0; a < enemyCount; a++)
			{
				var enemy = (ObjectStatus)gameModel.currentLevel.enemies[a];
				float xPos = (enemy.y + .5f - halfH) * gameModel.currentLevel.magnifier;
				float zPos = (enemy.x + .5f - halfW) * gameModel.currentLevel.magnifier;
			
				Vector3 pos = new Vector3 (xPos, 0f, zPos);

				createEnemySignal.Dispatch (enemy, 1, pos);
			}
			createPlayerSignal.Dispatch ();
		}
	}
}

