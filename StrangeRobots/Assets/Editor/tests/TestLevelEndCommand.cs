using System;
using NUnit.Framework;
using strange.examples.strangerobots.game;

namespace strange.examples.strangerobots.test
{
	public class TestLevelEndCommand
	{
		//Need to use explicit Signal. Possible weakness is Signals?
		UpdateLevelSignal updateLevelSignal = new UpdateLevelSignal ();
		//Arguably could use a Mock...but GameModel is just a simple data holder anyway.
		GameModel gameModel = new GameModel ();

		int INIT_VALUE = 0;

		[SetUp]
		public void Setup()
		{
			gameModel.level = INIT_VALUE;
		}

		[TearDown]
		public void Teardown()
		{
			gameModel.level = INIT_VALUE;
		}

		[Test]
		public void TestLevelEndCommandLevelInProgress()
		{
			gameModel.levelInProgress = true;
			LevelEndCommand command = new LevelEndCommand ();
			command.gameModel = gameModel;
			command.updateLevelSignal = updateLevelSignal;

			TestDelegate testDelegate = delegate()
			{
				command.Execute();
				Assert.AreEqual(INIT_VALUE + 1, gameModel.level);
				Assert.IsFalse(gameModel.levelInProgress);
			};

			updateLevelSignal.AddOnce (onLevelUpdatedLevelInProgress);
			Assert.DoesNotThrow (testDelegate);
		}

		[Test]
		public void TestLevelEndCommandLevelNotInProgress()
		{
			gameModel.levelInProgress = false;
			LevelEndCommand command = new LevelEndCommand ();
			command.gameModel = gameModel;
			command.updateLevelSignal = updateLevelSignal;

			TestDelegate testDelegate = delegate()
			{
				command.Execute();
				Assert.AreEqual(INIT_VALUE, gameModel.level);
			};

			updateLevelSignal.AddOnce (onLevelUpdatedLevelNotInProgress);
			Assert.DoesNotThrow (testDelegate);
		}

		private void onLevelUpdatedLevelInProgress(int newLevelValue)
		{
			Assert.AreEqual(INIT_VALUE + 1, newLevelValue);
		}

		private void onLevelUpdatedLevelNotInProgress(int newLevelValue)
		{
			Assert.AreEqual(INIT_VALUE, newLevelValue);
		}
	}
}