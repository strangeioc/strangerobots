using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class MoveEnemiesCommand : Command
	{

		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }
		
		[Inject]
		public IGameModel gameModel { get; set; }

		[Inject]
		public EnemyEndAnimationSignal enemyEndAnimationSignal{ get; set; }

		[Inject]
		public GameUtil gameUtil{ get; set; }

		private int index = 0;

		public override void Execute ()
		{
			Retain ();
			enemyEndAnimationSignal.AddListener (onAnimationComplete);
			moveEnemy ();
		}

		private void moveEnemy() {

			ObjectStatus enemy = gameModel.currentLevel.enemies [index];
			if (index < gameModel.currentLevel.enemies.Count) {
				Vector3 pos = gameUtil.positionInWorldSpace(enemy.x, enemy.y);


				EnemyView enemyView = enemy.view as EnemyView;
				enemyView.GoTo(pos);
			}
		}

		private void onAnimationComplete() {
			index ++;
			if (index >= gameModel.currentLevel.enemies.Count)
			{
				enemyEndAnimationSignal.RemoveListener (onAnimationComplete);
				Release ();
			} 
			else
			{
				moveEnemy();
			}
		}
	}
}