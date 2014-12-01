//The Context for our core game.

using System;
using strange.extensions.context.impl;
using UnityEngine;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;

namespace strange.examples.strangerobots.game
{
	public class GameContext : SignalContext
	{
		public GameContext (MonoBehaviour contextView) : base (contextView)
		{
		}

		// Create bindings as necessary to fulfill dependencies.
		// Anything nested inside if (Context.firstContext == this)
		// will be provided here *if* you just run this Context as a standalone app.
		// If this app runs as part of a multi-Context app, the bindings are assumed to
		// be provided elsewhere (from the MainContext, most likely).
		protected override void mapBindings ()
		{
			//We need to call mapBindings up the inheritance chain (see SignalContext)
			base.mapBindings ();

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)

			//If we're on mobile, we fulfill this as an empty dependency...
			//The job is fulfilled by OnscreenControlsView and its Mediator

			injectionBinder.Bind<IInput> ().To<NullInput> ().ToSingleton ();
#else

			//But if we're on desktop/editor/web, we map in a Keyboard controller.

			injectionBinder.Bind<IInput> ().To<KeyboardInput> ().ToSingleton ();
#endif

			//Injection
			if (Context.firstContext == this)
			{

				//In the multi-context world, this dependency is fulfilled Cross-Context by MainContext
				//(that way the same GameModel is shared between all Contexts).
				//When we run standalone, we need to provide it here.

				injectionBinder.Bind<IGameModel> ().To<GameModel> ().ToSingleton ();
			}
				
			injectionBinder.Bind<IGameConfig> ().To<GameConfig>().ToSingleton ();
			injectionBinder.Bind<GameUtil> ().ToSingleton ();

			//Pools
			//Pools provide a recycling system that makes the game much more efficient. Instead of destroying instances
			//(missiles/rocks/enemies/explosions) and re-instantiating them -- which is expensive -- we "checkout" the instances
			//from a pool, then return them when done.

			//These bindings setup the necessary pools, each as a Named injection, so we can tell the pools apart.
			injectionBinder.Bind<IPool<GameObject>>().To<Pool<GameObject>>().ToSingleton().ToName(GameElement.ENEMY_POOL);



			//Signals (not bound to Commands)
			//When a Signal isn't bound to a Command, it needs to be mapped, just like any other injected instance
			injectionBinder.Bind<GameStartedSignal> ().ToSingleton ();
			injectionBinder.Bind<LevelStartedSignal> ().ToSingleton ();

			if (Context.firstContext == this)
			{
				//These signals are provided by MainContext when we're in a multi-context situation
				injectionBinder.Bind<GameInputSignal> ().ToSingleton ();
				injectionBinder.Bind<UpdateLevelSignal> ().ToSingleton ();
				injectionBinder.Bind<UpdateLivesSignal> ().ToSingleton ();
				injectionBinder.Bind<UpdateScoreSignal> ().ToSingleton ();
				injectionBinder.Bind<EnemyEndAnimationSignal> ().ToSingleton ();
				injectionBinder.Bind<PlayerEndAnimationSignal> ().ToSingleton ();
			}




			//Commands
			//All Commands get mapped to a Signal that Executes them.
			if (Context.firstContext == this)
			{
				//Standalone
				commandBinder.Bind<StartSignal> ()
					.To<GameIndependentStartCommand> ()
					.Once ();
			}
			else
			{
				//Multi-Context
				commandBinder.Bind<StartSignal> ()
					.To<GameModuleStartCommand> ()
					.Once ();
			}

			//All the Signals/Commands necessary to play the game
			//Note:
			//1. Some of these are marked Pooled().
			//   Pooled Commands are more efficient when called repeatedly, but take up memory.
			//   Mark a Command as pooled if it will be called a lot...as in the main game loop.
			//2. Binding a Signal to a Command automatically maps the signal for injection.
			//   So it's only necessary to explicitly injectionBind Signals if they are NOT
			//   mapped to Commands.

			commandBinder.Bind<CreatePlayerSignal> ().To<CreatePlayerCommand> ();
			commandBinder.Bind<CreateEnemySignal> ().To<CreateEnemyCommand> ().Pooled();
			commandBinder.Bind<DestroyEnemySignal>().To<DestroyEnemyCommand>().Pooled();

			commandBinder.Bind<DestroyPlayerSignal>().To<DestroyPlayerCommand>().Pooled();

			commandBinder.Bind<GameStartSignal> ().To<GameStartCommand> ();
			commandBinder.Bind<GameEndSignal> ().To<EndGameCommand> ();

			//Notice how we can bind ONE Signal to SEVERAL Commands
			//This allows us to call Commands in sequence...ensuring that the second
			//Command only fires AFTER the first one has completed. This is especially
			//Useful for asynchronous calls, such as server communication.
			commandBinder.Bind<LevelStartSignal> ()
				.To<CreateGameFieldCommand>()
				.To<CleanupLevelCommand>()
				.To<StartLevelCommand> ()
				.To<MatchCameraToFieldCommand>()
				.InSequence();
			commandBinder.Bind<LevelEndSignal> ()
				.To<CleanupLevelCommand>()
				.To<LevelEndCommand> ()
				.InSequence();
			commandBinder.Bind<SetupLevelSignal> ()
				.To<SetupLevelCommand> ();
			commandBinder.Bind<StartTurnSignal> ()
				.To<StartTurnCommand>()
				.To<MovePlayerCommand>()
				.To<MoveEnemiesCommand> ()
				.To<EndTurnCommand> ()
				.InSequence();


			//Mediation
			//Mediation allows us to separate the View code from the rest of the app.
			//The details of **why** mediation is a good thing can be read in the faq:
			//http://thirdmotion.github.io/strangeioc/faq.html#why-mediator
			mediationBinder.Bind<EnemyView> ().To<EnemyMediator> ();
			mediationBinder.Bind<ExplosionView> ().To<ExplosionMediator> ();
			mediationBinder.Bind<GameDebugView> ().To<GameDebugMediator> ();
			mediationBinder.Bind<ShipView> ().To<ShipMediator> ();
			mediationBinder.Bind<ControlsView> ().To<ControlsMediator> ();

		}

		//After bindings are done, you sometimes want to do more stuff to configure your app.
		//Do that sort of stuff here.
		protected override void postBindings ()
		{
			//Establish our camera. We do this early since it gets injected in places that help us do layout.
			Camera cam = (contextView as GameObject).GetComponentInChildren<Camera> ();
			if (cam == null)
			{
				throw new Exception ("GameContext couldn't find the game camera");
			}
			injectionBinder.Bind<Camera> ().ToValue (cam).ToName (StrangeRobotsElement.GAME_CAMERA);

			// Configure the pools.
			// (Hint: all our pools for this game are identical, but for the content of the InstanceProvider)
			//Strange Pools by default "inflate" as necessary. This means that if you only
			//have one instance in the pool and require another, the pool will instantiate a second.
			//If you have two instances and need another, the pool will inflate again.
			//So long as you keep feeding instances back to the pool, there will always be enough instances,
			//and you'll never create more than you require.
			//The pool can use one of two inflation strategies:
			//1. DOUBLE (the default) is useful when your pool consists of objects that just exist in memory.
			//2. INCREMENT is better for onscreen objects as we're doing here. By INCREMENT-inflating, you'll
			//   get one new GameObject whenever you need one. This minimizes the necessary management of
			//   Views whenever the pool inflates.


			IPool<GameObject> rockPool = injectionBinder.GetInstance<IPool<GameObject>> (GameElement.ENEMY_POOL);
			rockPool.instanceProvider = new ResourceInstanceProvider ("Dalek", LayerMask.NameToLayer ("enemy"));
			rockPool.inflationType = PoolInflationType.INCREMENT;

			//Don't forget to call the base version...important stuff happens there!!!
			base.postBindings ();
		}
	}
}

