//Create the PlayerView

//NOTE: We're not using pools to create the PlayerView. Arguably we should...
//or at least store the single instance. As a practical matter, Ship creation
//and destruction is "rare" in game terms, happening only when the player gets
//killed or on the turnover of a level. The wastage of resources is therefore trivial.

using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class CreatePlayerCommand : Command
	{
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		[Inject]
		public IGameModel gameModel { get; set; }

		public override void Execute ()
		{
			if (injectionBinder.GetBinding<PlayerView> (GameElement.PLAYER_SHIP) != null)
				injectionBinder.Unbind<PlayerView> (GameElement.PLAYER_SHIP);

			//add the player's ship
			GameObject playerStyle = Resources.Load<GameObject> ("Doctor"/* GameElement.PLAYER_SHIP.ToString() */);
			//Add the controls
			GameObject controlsStyle = Resources.Load<GameObject> ("move_arrows");

			//shipStyle.transform.localScale = Vector3.one;

			GameObject playerGO = GameObject.Instantiate (playerStyle) as GameObject;
			GameObject constrolsGO = GameObject.Instantiate (controlsStyle) as GameObject;

			float xPos = (gameModel.currentLevel.player.y + .5f - (gameModel.currentLevel.height * .5f)) * gameModel.currentLevel.magnifier;
			float zPos = (gameModel.currentLevel.player.x + .5f - (gameModel.currentLevel.width * .5f)) * gameModel.currentLevel.magnifier;
			Vector3 pos = new Vector3 (xPos, 0f, zPos);


			playerGO.transform.localPosition = pos;
			constrolsGO.transform.localPosition = pos;

			ChangeLayersRecursively(playerGO.transform, "player");
			ChangeLayersRecursively(constrolsGO.transform, "control");

			playerGO.transform.parent = gameField.transform;
			constrolsGO.transform.parent = gameField.transform;

			constrolsGO.GetComponent<ControlsView>().target = playerGO.transform;

			injectionBinder.Bind<PlayerView> ().ToValue (playerGO.GetComponent<PlayerView> ()).ToName (GameElement.PLAYER_SHIP);

			//Whenever a ship is created, the game is on!
			gameModel.levelInProgress = true;
		}

		void ChangeLayersRecursively(Transform trans, string name)
		{
			trans.gameObject.layer = LayerMask.NameToLayer(name);
			foreach (Transform child in trans)
			{
				ChangeLayersRecursively(child, name);
			}
		}
	}
}

