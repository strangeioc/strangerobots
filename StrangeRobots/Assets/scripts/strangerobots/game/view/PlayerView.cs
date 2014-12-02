//The "View" for the player's ship. This MonoBehaviour is attached to the player_ship prefab inside Unity.

using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using strange.extensions.signal.impl;

namespace strange.examples.strangerobots.game
{
	public class PlayerView : GamePieceView
	{
		void OnCollisionEnter(Collision collision) {
			collisionSignal.Dispatch ();
		}
	}
}

