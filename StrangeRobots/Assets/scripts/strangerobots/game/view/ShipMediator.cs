//Mediators provide a buffer between Views and the rest of the app.
//THIS IS A REALLY GOOD THING. READ ABOUT IT HERE:
//http://thirdmotion.github.io/strangeioc/faq.html#why-mediator

//This mediates between the app and the ShipView.

using System;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class ShipMediator : Mediator
	{
		//View
		[Inject]
		public ShipView view { get; set; }
		
		[Inject]
		public DestroyPlayerSignal destroyPlayerSignal{ get; set; }
		
		[Inject]
		public PlayerEndAnimationSignal playerEndAnimationSignal{ get; set; }
		
		[Inject]
		public StartTurnSignal startTurnSignal { get; set; }

		//This is the first (important) thing to happen in the Mediator. It tells
		//you that your mediator has been attached, so it's like Start() or a
		//Constructor. Do all your startup stuff here
		public override void OnRegister ()
		{
			view.collisionSignal.AddListener (onCollision);
			view.endAnimationSignal.AddListener (onEndAnimation);

			view.Init ();
		}

		//OnRemove() is like a destructor/OnDestroy. Use it to clean up.
		public override void OnRemove ()
		{
			view.collisionSignal.RemoveListener (onCollision);
			view.endAnimationSignal.RemoveListener (onEndAnimation);
		}

		//When the View collides with something, dispatch the appropriate signal
		private void onCollision()
		{
			destroyPlayerSignal.Dispatch (view, false);
		}

		private void onEndAnimation() {
			playerEndAnimationSignal.Dispatch ();
		}
	}
}

