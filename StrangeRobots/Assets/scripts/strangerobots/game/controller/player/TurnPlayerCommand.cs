
using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class TurnPlayerCommand : Command
	{
		[Inject]
		public Vector2 coords{ get; set; }

		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }
		
		[Inject]
		public IGameModel gameModel { get; set; }
		
		public override void Execute ()
		{

		}
	}
}

