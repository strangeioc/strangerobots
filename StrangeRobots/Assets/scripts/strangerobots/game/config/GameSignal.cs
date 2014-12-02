//All the Signals exclusive to the GameContext

using System;
using strange.extensions.signal.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	//Game
	public class GameStartedSignal : Signal{}

	//Player
	public class CreatePlayerSignal : Signal{}

	public class PlayerEndAnimationSignal : Signal{}

	public class EnemyEndAnimationSignal : Signal{}

	//PlayerView - reference to the Player View
	//bool - False indicates destruction. True indicates cleanup at end of level.
	public class DestroyPlayerSignal : Signal<PlayerView, bool>{}

	//Enemies
	//int - The level (size) of the rock
	//Vector3 - Position of the rock
	public class CreateEnemySignal : Signal<ObjectStatus, int, Vector3>{}

	//EnemyView - reference to the specific enemy
	//bool - True indicates player gets points. False is simple cleanup.
	public class DestroyEnemySignal : Signal<EnemyView, bool>{}

	//Level
	public class SetupLevelSignal : Signal{}
	public class LevelStartedSignal : Signal{}

	//Turn
	public class StartTurnSignal : Signal<string>{}
	public class EndTurnSignal : Signal{}
}

