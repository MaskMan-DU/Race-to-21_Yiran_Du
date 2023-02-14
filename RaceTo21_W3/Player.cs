using System;
using System.Collections.Generic;

namespace RaceTo21
{
	public class Player
	{
		private string name;
		public List<Card> cards = new List<Card>(); // To store the player's hands
		public PlayerStatus status = PlayerStatus.active; // Keep the player's status
		public int score; // Keep the score of the player's hands
		public int points = 0; // To store the Points the player won

		public string Name
		{
			get { return name; }
		}


		public Player(string n)
		{
			//name = n;
			name = n;
        }

		/* Introduces player by name
		 * Called by CardTable object
		 */
		public void Introduce(int playerNum)
		{
			Console.WriteLine("Hello, my name is " + name + " and I am player #" + playerNum);
		}
	}
}

