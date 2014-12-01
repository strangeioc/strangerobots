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
			enemies = _config.enemies;
			player = _config.player;
		}
		
		public float magnifier { get; set; }
		
		public int width { get; set; }
		
		public int height { get; set; }
		
		public List<ObjectStatus> enemies { get; set; }
		
		public ObjectStatus player { get; set; }
		

	}
}