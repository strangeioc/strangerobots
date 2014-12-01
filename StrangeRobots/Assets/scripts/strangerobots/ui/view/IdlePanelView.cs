using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using strange.extensions.signal.impl;

namespace strange.examples.strangerobots.ui
{
	public class IdlePanelView : View
	{
		internal Signal proceedSignal = new Signal ();

		internal void Init()
		{
		}

		public void onStartClick()
		{
			proceedSignal.Dispatch ();
		}
	}
}

