
using System;
using strange.extensions.mediation.api;

namespace strange.examples.strangerobots.game
{
	public class ObjectStatus {
		public int x;
		public int y;
		
		public ObjectStatus(int xx, int yy) {
			x = xx;
			y = yy;
			destroyed = false;
		}

		public IView view{ get; set; }

		public bool destroyed { get; set; }
	}
}

