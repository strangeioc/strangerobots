//When the user picks a direction, compute all the outcomes first


using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strange.examples.strangerobots.game
{
	public class StartTurnCommand : Command
	{
		[Inject]
		public string direction{ get; set; }
		
		[Inject]
		public IGameModel gameModel { get; set; }
		
		public override void Execute ()
		{
			Debug.Log ("gameModel.level " + gameModel.level);


			LevelModel level = gameModel.currentLevel;
			ObjectStatus player = level.player;

			int xDest = player.x;
			int yDest = player.y;

			//Determine destination coords based on direction

			if (direction.IndexOf ("N") > -1)
			{
				yDest -= 1;
			} else if (direction.IndexOf ("S") > -1)
			{
				yDest += 1;
			}
			if (direction.IndexOf ("W") > -1)
			{
				xDest -= 1;
			} else if (direction.IndexOf ("E") > -1)
			{
				xDest += 1;
			}
			//Confirm player can go in the chosen direction
			bool legalMove = true;
			if (xDest < 0 || yDest < 0 || xDest >= level.width || yDest >= level.height)
			{
				legalMove = false;
			}
			int aa = level.enemies.Count;
			for (int a = 0; a < aa; a++)
			{
				if (level.enemies[a].x == xDest && level.enemies[a].y == yDest) {
					legalMove = false;
					break;
				}
			}

			//If yes...
			if (legalMove)
			{
				//Move player
				player.x = xDest;
				player.y = yDest;

				level.player = player;

				//Move all enemies iteratively, recording kills
				int bb = level.enemies.Count;
				for (int b = 0; b < bb; b++)
				{
					ObjectStatus enemy = level.enemies[b];

					if (enemy.destroyed)
						continue;

					int enemyXPos = enemy.x;
					int enemyYPos = enemy.y;

					int xDist = enemy.x - player.x;
					int yDist = enemy.y - player.y;

					float theta = Mathf.Atan2((float)yDist, (float)xDist) * Mathf.Rad2Deg;

					if (theta >= -45 && theta <= 45) {
						enemyXPos -= 1;
					}
					else if (theta >= 135 || theta <= -135) {
						enemyXPos += 1;
					}

					if (theta >= 45 && theta <= 135) {
						enemyYPos -= 1;
					}
					else if (theta <= -45 && theta >= -135) {
						enemyYPos += 1;
					}
					enemy.x = enemyXPos;
					enemy.y = enemyYPos;
					level.enemies[b] = enemy;


					//loop to mark collisions
					for (int c = 0; c < bb; c++) {
						ObjectStatus other = level.enemies[c];
						if (b != c && other.x == enemy.x && other.y == enemy.y) {
							other.destroyed = enemy.destroyed = true;
						}
					}
				}




				//Note if player was killed
			}
			//If no...
			else
			{
				//Reject?
			}


		}
	}
}
