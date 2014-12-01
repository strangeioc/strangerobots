
using System;
using strange.extensions.command.impl;
using UnityEngine;


namespace strange.examples.strangerobots.game
{
	public class MatchCameraToFieldCommand : Command
	{
		
		[Inject]
		public IGameConfig config { get; set; }
		
		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public IScreenUtil screenUtil { get; set; }

		[Inject(StrangeRobotsElement.GAME_CAMERA)]
		public Camera gameCamera{ get; set; }

		public override void Execute() {
			ILevelConfig level = config.getLevel (gameModel.level);
			Vector3 dest = screenUtil.FillFrustum (level.width * 5f, level.height * 5f);
			gameCamera.gameObject.GetComponent<CameraView> ().GoToPosition (dest);
			
		}
	}
}

