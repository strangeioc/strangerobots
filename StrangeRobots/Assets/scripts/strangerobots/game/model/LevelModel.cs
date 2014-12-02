//Data model of a level (in progress)

using System;
using System.Collections.Generic;

namespace strange.examples.strangerobots.game
{
	public class LevelModel
	{
		ILevelConfig _config;

		public LevelModel (ILevelConfig config)
		{
			_config = config;
			Reset ();
		}

		public void Reset ()
		{
			magnifier = _config.magnifier;
			width = _config.width;
			height = _config.height;
			enemies = new List<ObjectStatus>();
			for (int a = 0, aa = _config.enemies.Count; a < aa; a++)
			{
				enemies.Add (new ObjectStatus(_config.enemies[a].x, _config.enemies[a].y));
			}
			player = new ObjectStatus(_config.player.x, _config.player.y);
		}
		
		public float magnifier { get; set; }
		
		public int width { get; set; }
		
		public int height { get; set; }
		
		public List<ObjectStatus> enemies { get; set; }
		
		public ObjectStatus player { get; set; }
		

	}
}