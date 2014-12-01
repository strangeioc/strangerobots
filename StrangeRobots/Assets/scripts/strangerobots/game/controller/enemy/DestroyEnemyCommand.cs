//A Rock is destroyed if it is struck by a player missile.
//It may also get removed during cleanup when re-starting the game.

//One could make a pretty good argument that this Command should in fact be broken into three:
//1. Tabulate points.
//2. Destroy the Rock.
//3. Determine if the level is complete.

//I chose not to do that in this case (a bit lazy, to be honest). If I did, it would be a fairly simple matter,
//and probably better, since I'd be doing the single-responsibility thing far more cleanly.
//In the GameContext, I'd re-write the binding something like this:

//Currently:
//commandBinder.Bind<DestroyEnemySignal>().To<DestroyEnemyCommand>().Pooled();

//Rewritten:
//commandBinder.Bind<DestroyRockSignal>()
//		.To<ScoreByRockCommand>()
//		.To<DestroyRockCommand>()
//		.To<CheckLevelEndCommand>()
//		.InSequence()
//		.Pooled();

using System;
using strange.extensions.command.impl;
using UnityEngine;
using System.Collections;
using strange.extensions.pool.api;

namespace strange.examples.strangerobots.game
{
	public class DestroyEnemyCommand : Command
	{
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		//A reference to the specific rock
		[Inject]
		public EnemyView enemyView{ get; set; }

		//Does this destruction earn the player points?
		[Inject]
		public bool isPointEarning{ get; set; }

		//For score-keeping
		[Inject]
		public IGameModel gameModel{ get; set; }

		//We run a brief coroutine after destruction to test whether all Rocks have been destroyed
		[Inject]
		public IRoutineRunner routineRunner{ get; set; }

		//We're drawing instances from a pool, instead of wasting our resources.
		[Inject(GameElement.ENEMY_POOL)]
		public IPool<GameObject> pool{ get; set; }

		[Inject]
		public UpdateScoreSignal updateScoreSignal{ get; set; }

		[Inject]
		public CreateEnemySignal createEnemySignal{ get; set; }

		[Inject]
		public LevelEndSignal levelEndSignal{ get; set; }

		[Inject]
		public IGameConfig gameConfig{ get; set; }

		private static Vector3 PARKED_POS = new Vector3(1000f, 0f, 1000f);


		public override void Execute ()
		{
			if (isPointEarning)
			{
				//NOTE: arguably all the point-earning from destroying Rocks and Enemies
				//should be offloaded to a set of ScoreCommands. Certainly in a more complex game,
				//You'd do yourself a favor by centralizing the tabulation of scores.
				int level = gameModel.level;

				gameModel.score += gameConfig.baseRockScore * level;
				updateScoreSignal.Dispatch (gameModel.score);

				Vector3 pos = enemyView.transform.position;
				GameObject explosionStyle = Resources.Load<GameObject> ("player_explosion");
				GameObject explosionGO = GameObject.Instantiate (explosionStyle) as GameObject;

				explosionGO.transform.localPosition = pos;
				explosionGO.transform.parent = gameField.transform;
			}

			//We're pooling instances, not actually destroying them,
			//So reset the instances to an appropriate state for reuse...
			enemyView.ResetAll();
			enemyView.gameObject.SetActive (false);

			//...and store them offscreen
			enemyView.transform.localPosition = PARKED_POS;
			pool.ReturnInstance (enemyView.gameObject);

			if (isPointEarning)
			{
				//If this was the player destroying a rock, pause...
				Retain ();
				routineRunner.StartCoroutine (checkEnemies ());
			}
		}

		//...then test if we've destroyed them all.
		public IEnumerator checkEnemies()
		{
			//A one-frame delay is necessary to ensure the gameField's view has cleaned up the destroyed rock.
			yield return null;

			EnemyView[] enemies = gameField.GetComponentsInChildren<EnemyView> ();
			bool levelCleared = true;
			foreach (EnemyView enemy in enemies)
			{
				if (enemy.gameObject.activeSelf)
				{
					levelCleared = false;
				}
			}
			if (levelCleared)
			{
				levelEndSignal.Dispatch ();
			}
			Release ();
		}
	}
}

