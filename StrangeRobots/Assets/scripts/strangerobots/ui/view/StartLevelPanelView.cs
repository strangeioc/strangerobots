using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.signal.impl;

namespace strange.examples.strangerobots.ui
{
	public class StartLevelPanelView : View
	{

		public Text level_field;

		internal Signal proceedSignal = new Signal ();

		internal void Init()
		{
		}

		internal void SetLevel(int value)
		{
			level_field.text = value.ToString ();
		}

		public void onStartClick()
		{
			proceedSignal.Dispatch ();
		}
	}
}

