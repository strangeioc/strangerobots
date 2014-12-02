//The "View" for the player's ship. This MonoBehaviour is attached to the player_ship prefab inside Unity.

using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using strange.extensions.signal.impl;

namespace strange.examples.strangerobots.game
{
	public class ControlsView : View
	{
		[Inject(StrangeRobotsElement.GAME_CAMERA)]
		public Camera gameCamera{ get; set; }

		public Transform target;
		internal Signal<string> moveSignal = new Signal<string> ();

		public List<GameObject> controls;

		private bool acceptingInput = true;

		//Initialize called by the Mediator. Init is a little like
		//Start()...but by calling it from the Mediator's OnRegister(),
		//we know the Mediator is in place before doing anything important.
		internal void Init()
		{
		}
		
		//Move based on user input
		void Update()
		{
			if (Input.GetMouseButtonDown(0) && acceptingInput) {
				Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray.origin, ray.direction, out hit, 2000) && controls.Contains(hit.collider.gameObject))
				{
					moveSignal.Dispatch (hit.collider.gameObject.name);
				}
			}
			if (target)
				transform.position = target.position;
		}
	}
}