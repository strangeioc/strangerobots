using System;
using System.Collections;

namespace strange.examples.strangerobots.game
{
	public interface IGameConfig
	{
		int initLives{ get; set; }

		int newLifeEvery{ get; set; }

		int initRocks{ get; set; }

		int additionalRocksPerLevel{ get; set; }

		int baseRockScore{ get; set; }

		float rockExplosiveForceMin{ get; set; }

		float rockExplosiveForceMax{ get; set; }

		float rockExplosiveRadius{ get; set; }

		float enemySpawnSecondsMin{ get; set; }

		float enemySpawnSecondsMax{ get; set; }

		int baseEnemyScore{ get; set; }

		ArrayList levels{ get; set; }

		ILevelConfig getLevel(int level);
	}
}

