
using System;
using System.Collections.Generic;
namespace strange.examples.strangerobots.game
{
	public interface ILevelConfig
	{
		float magnifier { get; }

		int width{ get; }

		int height{ get; }

		List<ObjectStatus> enemies{ get; }

		ObjectStatus player { get; }
	}
}

