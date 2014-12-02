using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class MovePlayerCommand : Command
	{
		[Inject]
		public string direction { get; set; }

		[Inject(GameElement.PLAYER_SHIP)]
		public PlayerView view { get; set; }
		
		[Inject]
		public IGameModel gameModel { get; set; }

		[Inject]
		public PlayerEndAnimationSignal playerEndAnimationSignal{ get; set; }

		[Inject]
		public GameUtil gameUtil{ get; set; }
		
		public override void Execute ()
		{
			Retain ();
			playerEndAnimationSignal.AddListener (onPlayerAnimationComplete);


			ObjectStatus player = gameModel.currentLevel.player;

			Vector3 pos = gameUtil.positionInWorldSpace (player.x, player.y);

			view.GoTo (pos);
		}

		private void onPlayerAnimationComplete() {
			playerEndAnimationSignal.RemoveListener (onPlayerAnimationComplete);
			Release ();
		}
	}
}
