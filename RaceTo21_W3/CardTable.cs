using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class CardTable
    {
        public CardTable()
        {
            Console.WriteLine("Setting Up Table...");
        }

        /* Shows the name of each player and introduces them by table position.
         * Is called by Game object.
         * Game object provides list of players.
         * Calls Introduce method on each player object.
         */
        public void ShowPlayers(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Introduce(i + 1); // List is 0-indexed but user-friendly player positions would start with 1...
            }
        }

        /* Gets the user input for number of players.
         * Is called by Game object.
         * Returns number of players to Game object.
         */
        public int GetNumberOfPlayers()
        {
            Console.Write("How many players? ");
            string response = Console.ReadLine();
            int numberOfPlayers;
            while (int.TryParse(response, out numberOfPlayers) == false
                || numberOfPlayers < 2 || numberOfPlayers > 8)
            {
                Console.WriteLine("Invalid number of players.");
                Console.Write("How many players?");
                response = Console.ReadLine();
            }
            return numberOfPlayers;
        }

        /* Gets the name of a player
         * Is called by Game object
         * Game object provides player number
         * Returns name of a player to Game object
         */
        public string GetPlayerName(int playerNum)
        {
            Console.Write("What is the name of player# " + playerNum + "? ");
            string response = Console.ReadLine();
            while (response.Length < 1)
            {
                Console.WriteLine("Invalid name.");
                Console.Write("What is the name of player# " + playerNum + "? ");
                response = Console.ReadLine();
            }
            return response;
        }

        /// <summary>
        /// To get answer from the player, how many cards do you want?
        /// </summary>
        /// <param name="player">Game object provides the player's data</param>
        /// <returns>The number of cards the player want to draw</returns>
        /// Is called by Game object
        public int OfferHowManyCards(Player player)
        {
            while (true)
            {
                Console.Write(player.Name + ", how many cards do you want? (0-3)");
                string response = Console.ReadLine();
                if (int.TryParse(response, out int howManyCards))
                {
                    if (howManyCards < 4 && howManyCards >= 0)
                    {
                        return howManyCards;
                    }
                    else
                    {
                        Console.WriteLine("Please type 0, 1, 2 or 3!");
                    }
                }
                else
                {
                    Console.WriteLine("Please type 0, 1, 2 or 3!");
                }
            }
        }

        /// <summary>
        /// Display information about this player, the player's name, the hands the player has, the score, the status and the points.
        /// </summary>
        /// <param name="player">Game object provides the player's data</param>
        /// Is called by Game object
        public void ShowHand(Player player)
        {   
            if (player.cards.Count > 0) // When the player has cards, running the following code
            {
                Console.Write(player.Name + " has: ");

                string allCards = ""; // Adjust: Use string allCards to store all cards in the player hands

                foreach (Card card in player.cards)
                {
                    // Console.Write(card.fullName + ", ");
                    allCards = allCards + card.FullName + ", "; // Adjust: allCards will save store the full name of all the cards + ", "
                }

                Console.WriteLine(allCards.Remove(allCards.Length - 2) + " = " + player.score + "/21 "); // Adjust: Use Remove function to remove the final ", " of allCards variable

                Console.WriteLine(player.Name + "'s points: " + player.points);

                if (player.status != PlayerStatus.active) // When the player status is not active, output the player's status
                {
                    Console.Write("(" + player.status.ToString().ToUpper() + ")");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Run the ShowHand() method for each player
        /// </summary>
        /// <param name="players">Game object provides list of players</param>
        /// Is called by Game object
        public void ShowHands(List<Player> players)
        {
            foreach (Player player in players)
            {
                ShowHand(player);
            }
        }

        /// <summary>
        /// Show the winner of each round, and the points of the winner
        /// </summary>
        /// <param name="player">Game object provides the player's data</param>
        /// Is called by Game object
        public void AnnounceWinner(Player player)
        {
            // Remove the detection here

            Console.WriteLine(player.Name + " wins!");

            Console.WriteLine(player.Name + "'s points: " + player.points);

        }

        /// <summary>
        /// Type the text "No player draws card!"
        /// </summary>
        /// Is called by Game object
        public void resultForNoDrawnCard()
        {
            Console.WriteLine("No player draws card!");
        }
    }
}