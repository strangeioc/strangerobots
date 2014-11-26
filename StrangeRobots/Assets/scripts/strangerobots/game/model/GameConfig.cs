using System;
using System.Collections;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class GameConfig : IGameConfig
	{
		//PostConstruct methods fire automatically after Construction
		//and after all injections are satisfied. It's a safe place
		//to do things you'd usually sonsider doing in the Constructor.
		[PostConstruct]
		public void PostConstruct()
		{
			TextAsset file = Resources.Load ("gameConfig") as TextAsset;

			var n = SimpleJSON.JSON.Parse (file.text);

			initLives = n ["initLives"].AsInt;
			newLifeEvery = n ["newLifeEvery"].AsInt;
			maxLives = n ["maxLives"].AsInt;


			additionalRocksPerLevel = n ["additionalRocksPerLevel"].AsInt;
			baseRockScore = n ["baseRockScore"].AsInt;
			rockExplosiveForceMin = n ["rockExplosiveForceMin"].AsFloat;
			rockExplosiveForceMax = n ["rockExplosiveForceMax"].AsFloat;

			baseEnemyScore = n ["baseEnemyScore"].AsInt;

			SimpleJSON.JSONArray lvls = n ["levels"].AsArray;

			levels = new ArrayList ();

			for (int a  = 0; a < lvls.Count; a++)
			{
				int width = lvls[a]["dimensions"]["width"].AsInt;
				int height = lvls[a]["dimensions"]["height"].AsInt;

				SimpleJSON.JSONArray nms = lvls[a]["enemies"].AsArray;
				ArrayList enemies = new ArrayList();

				for (int b = 0; b < nms.Count; b++) {
					int x = nms[b]["x"].AsInt;
					int y = nms[b]["y"].AsInt;
					enemies.Add(new EnemyInit(x, y));
				}


				levels.Add(new LevelConfig(width, height, enemies));

			}
		}

		#region implement IGameConfig
		public int initLives{ get; set; }

		//Unimplemented
		public int newLifeEvery{ get; set; }

		//Unimplemented
		public int maxLives{ get; set; }

		public int initRocks{ get; set; }

		public int additionalRocksPerLevel{ get; set; }

		public int baseRockScore{ get; set; }

		public float rockExplosiveForceMin{ get; set; }

		public float rockExplosiveForceMax{ get; set; }

		public float rockExplosiveRadius{ get; set; }

		public float enemySpawnSecondsMin{ get; set; }

		public float enemySpawnSecondsMax{ get; set; }

		public int baseEnemyScore{ get; set; }

		public ArrayList levels { get; set; }

		public ILevelConfig getLevel(int value) {
			return levels[value] as ILevelConfig;
		}
		#endregion
	}
}

