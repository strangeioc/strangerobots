using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using strange.extensions.signal.impl;

namespace strange.examples.strangerobots.game
{
	public class GamePieceView : View
	{
		
		internal Signal collisionSignal = new Signal ();
		internal Signal endAnimationSignal = new Signal ();
		
		//Settable from Unity
		public float journeySeconds = .5f;
		
		private int phase = 0;
		
		private Vector3 startPos;
		private Vector3 endPos;
		
		private Quaternion startRot;
		private Quaternion endRot;
		
		
		private float startTime = 0;
		private float journeyDistance = 0;
		
		
		
		//When the user selects input (by whatever method we've mapped), the result
		//arrives in the form of an int. We keep a copy of that int here for analysis.
		//The value is bitwise...see KeyboardInput and OnscreenControlsView for details.
		private int input;
		
		//Initialize called by the Mediator. Init is a little like
		//Start()...but by calling it from the Mediator's OnRegister(),
		//we know the Mediator is in place before doing anything important.
		internal void Init()
		{

			
			
			startPos = endPos = transform.position;
			startRot = endRot = transform.rotation;
		}
		
		internal void GoTo(Vector3 pos) {
			//Start/end positions
			startPos = transform.position;
			endPos = pos;
			
			//Start/end rotations
			startRot = transform.rotation;
			float theta = Mathf.Atan2(endPos.x - startPos.x, endPos.z - startPos.z) * Mathf.Rad2Deg - 90f;
			endRot = Quaternion.Euler (new Vector3 (0f, theta, 0f));
			
			phase = 1;
			startTime = Time.time;
		}
		
		//Move based on user input
		void FixedUpdate()
		{
			if (startTime != 0)
			{
				if (phase == 1) {
					float fracJourney = (Time.time - startTime) / journeySeconds;
					Quaternion q = Quaternion.Slerp (startRot, endRot, fracJourney);
					transform.rotation = q;
					Quaternion invertQ = Quaternion.Inverse(q);
					if (Quaternion.Angle(transform.rotation, endRot) < .1f) {
						startTime = Time.time;
						transform.rotation = endRot;
						phase = 2;
					}
				}
				if (phase == 2) {
					float fracJourney = (Time.time - startTime) / journeySeconds;
					Vector3 pos = Vector3.Lerp (startPos, endPos, fracJourney);
					transform.position = pos;
					
					if (Vector3.Distance(pos, endPos) < .1f) {
						startTime = 0;
						phase = 0;
						endAnimationSignal.Dispatch();
					}
				}
			}
		}
		
		//If we hit anything, we fire a collisionSignal. The logic of what happens passes through the ShipMediator to the DestroyPlayerCommand;
		//but we could map it to something else if we were inclined to change the logic. For example, if we were to add rubber bumpers,
		//the mediator could add a playerBounceSignal() mapped (if necessary) to an appropriate PlayerBounceCommand. Importantly, the View
		//wouldn't require an edit for that rule change to happen.
		void OnTriggerEnter()
		{
			collisionSignal.Dispatch ();
		}
	}
}

