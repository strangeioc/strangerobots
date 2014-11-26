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
		public CreateRockSignal createRockSignal{ get; set; }

		[Inject]
		public IGameConfig gameConfig{ get; set; }

		public override void Execute ()
		{
			createPlayerSignal.Dispatch ();

			ILevelConfig level = gameConfig.getLevel (gameModel.level);

			var halfW = (float)level.width * .5f;
			var halfH = (float)level.height * .5f;

			int enemyCount = level.enemies.Count;
			for (int a = 0; a < enemyCount; a++)
			{
				var enemy = (EnemyInit)level.enemies[a];
				float x = (enemy.x + .5f - halfW) * 5f;
				float y = (enemy.y + .5f - halfH) * 5f;
				Vector3 pos = new Vector3 (x, 0f, y);

				createRockSignal.Dispatch (1, pos);
			}
		}
	}
}

