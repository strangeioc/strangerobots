//Newly created Rocks get pulled from their pool and placed at the given position.
//(See DestroyEnemyCommand for the bit where they're returned to their pool)

using System;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.pool.api;

namespace strange.examples.strangerobots.game
{
	public class CreateEnemyCommand : Command
	{
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		//We're drawing instances from a pool, instead of wasting our resources.
		[Inject(GameElement.ENEMY_POOL)]
		public IPool<GameObject> pool{ get; set; }


		[Inject]
		public int level{ get; set; }

		[Inject]
		public ObjectStatus enemy{ get; set; }

		//The position to place the Rock
		[Inject]
		public Vector3 pos{ get; set; }

		[Inject]
		public IGameConfig gameConfig{ get; set; }

		public override void Execute ()
		{
			//Draw an instance from the Pool
			GameObject enemyGO = pool.GetInstance();
			enemyGO.SetActive (true);

			//place it
			enemyGO.transform.position = pos;
			enemyGO.layer = LayerMask.NameToLayer ("enemy");
			//enemyGO.GetComponent<EnemyView> ().level = level;

			enemyGO.transform.parent = gameField.transform;
			enemy.view = enemyGO.GetComponent<EnemyView>();
			(enemy.view as MonoBehaviour).enabled = true;
		}
	}
}

