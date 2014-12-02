
//Mediators provide a buffer between Views and the rest of the app.
//THIS IS A REALLY GOOD THING. READ ABOUT IT HERE:
//http://thirdmotion.github.io/strangeioc/faq.html#why-mediator

//This mediates between the app and the PlayerView.

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
		
		[Inject]
		public EndTurnSignal endTurnSignal { get; set; }
		
		[Inject]
		public LevelStartedSignal levelStartedSignal { get; set; }
		
		[Inject]
		public GameEndSignal gameEndSignal { get; set; }

		private bool blocked = false;
		
		//This is the first (important) thing to happen in the Mediator. It tells
		//you that your mediator has been attached, so it's like Start() or a
		//Constructor. Do all your startup stuff here
		public override void OnRegister ()
		{
			view.Init ();
			view.moveSignal.AddListener (onMove);
			endTurnSignal.AddListener (onEndTurn);
			levelStartedSignal.AddListener (show);
			gameEndSignal.AddListener (onGameEnd);
		}
		
		//OnRemove() is like a destructor/OnDestroy. Use it to clean up.
		public override void OnRemove ()
		{
			view.moveSignal.RemoveListener (onMove);
			endTurnSignal.RemoveListener (onEndTurn);
			levelStartedSignal.RemoveListener (show);
			gameEndSignal.RemoveListener (onGameEnd);
		}

		private void onEndTurn() {
			if (!blocked)
			{
				show ();
			}
		}
		
		//When a click on the view indicates a user command
		private void onMove(string direction) {
			hide ();
			startTurnSignal.Dispatch (direction);
		}

		private void onGameEnd() {
			blocked = true;
			hide ();
		}
		
		private void hide() {
			gameObject.SetActive(false);
		}
		
		private void show() {
			gameObject.SetActive(true);
		}
	}
}