
using System;
using System.Collections;
namespace strange.examples.strangerobots.game
{
	public struct EnemyInit {
		public int x;
		public int y;

		public EnemyInit(int xx, int yy) {
			x = xx;
			y = yy;
		}
	}


	public class LevelConfig : ILevelConfig
	{
		private int _width;
		private int _height;
		private ArrayList _enemies;


		public LevelConfig (int width, int height, ArrayList enemies)
		{
			_width = width;
			_height = height;
			_enemies = enemies;
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
		
		public ArrayList enemies{ 
			get {
				return _enemies;
			}
		}
	}
}

