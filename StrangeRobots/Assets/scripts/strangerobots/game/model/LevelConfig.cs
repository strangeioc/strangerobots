
using System;
using System.Collections.Generic;
namespace strange.examples.strangerobots.game
{
	public class LevelConfig : ILevelConfig
	{
		private float _magnifier;
		private int _width;
		private int _height;
		private List<ObjectStatus> _enemies;
		private ObjectStatus _player;


		public LevelConfig (float magnifier, int width, int height, List<ObjectStatus> enemies, ObjectStatus player)
		{
			_magnifier = magnifier;
			_width = width;
			_height = height;
			_enemies = enemies;
			_player = player;
		}

		public float magnifier { 
			get {
				return _magnifier;
			}
		}

		public int width{
			get {
				return _width;
			}
		}
		
		public int height{
			get {
				return _height;
			}
		}
		
		public List<ObjectStatus> enemies{ 
			get {
				return _enemies;
			}
		}

		public ObjectStatus player {
			get {
				return _player;
			}
		}
	}
}

