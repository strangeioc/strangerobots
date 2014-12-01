//The "View" for a Rock. This MonoBehaviour is attached to the rock prefab inside Unity.

using System;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class EnemyView : GamePieceView
	{
		void OnCollisionEnter(Collision collision) {
			EnemyView other = collision.collider.gameObject.GetComponent<EnemyView> ();
			if (other)
				collisionSignal.Dispatch ();
		}
	}
}

