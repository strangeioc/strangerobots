
//Mediators provide a buffer between Views and the rest of the app.
//THIS IS A REALLY GOOD THING. READ ABOUT IT HERE:
//http://thirdmotion.github.io/strangeioc/faq.html#why-mediator

//This mediates between the app and the ShipView.

using System;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class ControlsMediator : Mediator
	{
		//View
		[Inject]
		public ControlsView view { get; set; }
		
		//Signals
		
		[Inject]
		public StartTurnSignal startTurnSignal { get; set; }
		
		//This is the first (important) thing to happen in the Mediator. It tells
		//you that your mediator has been attached, so it's like Start() or a
		//Constructor. Do all your startup stuff here
		public override void OnRegister ()
		{
			view.Init ();
			view.moveSignal.AddListener (onMove);
		}
		
		//OnRemove() is like a destructor/OnDestroy. Use it to clean up.
		public override void OnRemove ()
		{
			view.moveSignal.RemoveListener (onMove);
		}
		
		//When a click on the view indicates a user command
		private void onMove(string direction) {
			startTurnSignal.Dispatch (direction);
		}
	}
}