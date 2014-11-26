
using System;
using System.Collections;
namespace strange.examples.strangerobots.game
{
	public interface ILevelConfig
	{
		int width{ get; }

		int height{ get; }

		ArrayList enemies{ get; }
	}
}

