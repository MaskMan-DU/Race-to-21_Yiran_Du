using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class Game
    {
        private int numberOfPlayers; // number of players in current game
        private List<Player> players = new List<Player>(); // list of objects containing player data
        
        private List<Player> giveUpPlayers = new List<Player>(); // To keep the data of players who give up

        private CardTable cardTable; // object in charge of displaying game information
        private Deck deck = new Deck(); // deck of cards
        private int currentPlayer = 0; // current player on list
        public Tasks nextTask; // keeps track of game state
        private readonly bool cheating = false; // lets you cheat for testing purposes if true
        public bool Cheating { get { return cheating; } } // Use this to keep "cheating" readonly

        private int highPoints = 0; // Implementation: Set a variable to keep track the high points (part of Level2)
        private int pointsToGameOver = 60; // Implementation: Set a variable to determine the score that needs to be reached to end the game

        public Game(CardTable c)
        {
            cardTable = c;
            deck.buildDeck();
            deck.Shuffle();
            // deck.ShowAllCards();
            Console.WriteLine("*****************"); 
            Console.WriteLine("The final winner will be the one who has more than " + pointsToGameOver + " points!");
            Console.WriteLine("If everyone quits early, or if only one player is present, the player who has the highest points wins！");
            /*Console.WriteLine("*****************");
            Console.WriteLine("Do you want to open cheat mode? (Y/N)");
            string response = Console.ReadLine();
            while (true)
            {
                if (response.ToUpper().StartsWith("Y"))
                {
                    cheating = true;
                    return;
                }
                else if (response.ToUpper().StartsWith("N"))
                {
                    cheating = false;
                    return;
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                }
            }*/

            nextTask = Tasks.GetNumberOfPlayers;
        }

        /* Adds a player to the current game
         * Called by DoNextTask() method
         */
        private void AddPlayer(string n)
        {
            players.Add(new Player(n));
        }

        /* Figures out what task to do next in game
         * as represented by field nextTask
         * Calls methods required to complete task
         * then sets nextTask.
         */
        public void DoNextTask()
        {
            Console.WriteLine("================================"); // this line should be elsewhere right?
            if (nextTask == Tasks.GetNumberOfPlayers)
            {
                numberOfPlayers = cardTable.GetNumberOfPlayers();
                nextTask = Tasks.GetNames;
            }
            else if (nextTask == Tasks.GetNames)
            {
                for (var count = 1; count <= numberOfPlayers; count++)
                {
                    var name = cardTable.GetPlayerName(count);
                    AddPlayer(name); // NOTE: player list will start from 0 index even though we use 1 for our count here to make the player numbering more human-friendly
                }
                nextTask = Tasks.IntroducePlayers;
            }
            else if (nextTask == Tasks.IntroducePlayers)
            {
                cardTable.ShowPlayers(players);
                nextTask = Tasks.PlayerTurn;
            }
            else if (nextTask == Tasks.PlayerTurn)
            {
                cardTable.ShowHands(players);
                Player player = players[currentPlayer];
                if (player.status == PlayerStatus.active)
                {
                    /* Implementation:
                     * A player can choose to draw up to 3 cards each turn, but they get
                     * all cards at once; they don’t get to decide after each card
                     */
                    int drawnCardNumber = cardTable.OfferHowManyCards(player);

                    if (drawnCardNumber != 0) // If the player want to draw cards
                    {
                        
                        for (int i = 0; i < drawnCardNumber; i++) // Draw the number of cards indicated by the player
                        {
                            Card card = deck.DealTopCard();
                            player.cards.Add(card);
                        }

                        player.score = ScoreHand(player);
                        

                        if (player.score > 21) // The player bust
                        {
                                player.status = PlayerStatus.bust;

                                int losePoints = player.score - 21; // Implementation: If the player is bust, the player loses points equal to their hand total minus 21. (Level 2)
                                player.points -= losePoints;
                                cardTable.ShowHand(player);

                        }
                        else if (player.score == 21) // The player win
                        {
                                player.status = PlayerStatus.win;
                                cardTable.ShowHand(player);

                        }
                        else // The player still active
                        {
                            cardTable.ShowHand(player);
                            Console.Write("Do you want to stay? (Y/N)");
                            string response = Console.ReadLine();
                            if (response.ToUpper().StartsWith("Y")) // The player want to stay
                            {
                                player.status = PlayerStatus.stay;
                            }
                            else if (response.ToUpper().StartsWith("N")) // The player still want to play
                            {
                                player.status = PlayerStatus.active; 
                            }
                            else
                            {
                                Console.WriteLine("Please answer Y(es) or N(o)!");
                            }

                        }           
                    }
                    else // The player do not want to draw cards at the beginning of the round.
                    {
                        player.status = PlayerStatus.stay;
                    }

                }
                
                nextTask = Tasks.CheckForEnd;
            }
            else if (nextTask == Tasks.CheckForEnd)
            {
                if (!CheckActivePlayers()) // No players' status is active
                {
                    Player winner = DoFinalScoring(); // Get the winner
  
                    if (winner != null) // Implementation: When a player win, the player earns the points equal to his score. (Level 1)
                    {
                        winner.points += winner.score; 
                    }
                   

                    // Adjust: Put the winner detection out of the AnnounceWinner method. If no player draws card, the game will not stop.
                    if (winner != null) // If there is a winner
                    {
                        cardTable.AnnounceWinner(winner);
                        IsContinue(); // Check if the game can still continue
                    }
                    else // There is no winner
                    {
                        cardTable.resultForNoDrawnCard(); // Add a new method to told all players that nobody draws card.
                        // Reset all players status
                        foreach (var player in players)
                        {
                            player.status = PlayerStatus.active;
                        }

                        if (currentPlayer == (players.Count - 1)) // If the current player is the final player, reset the variable currentPlayer
                        {
                            currentPlayer = 0; // back to the first player...
                        }

                        nextTask = Tasks.PlayerTurn;
                    }

                }
                else
                {
                    currentPlayer++;
                    
                    nextTask = Tasks.PlayerTurn;
                }
            }
            else // we shouldn't get here...
            {
                Console.WriteLine("I'm sorry, I don't know what to do now!");
                nextTask = Tasks.GameOver;
            }
        }

        /// <summary>
        /// Calculate the total score of the player's hand.
        /// </summary>
        /// <param name="player">The player's data</param>
        /// <returns>the total score of the player's hand</returns>
        /// Is called by DoNextTask() method
        private int ScoreHand(Player player)
        {
            int score = 0;

            if (Cheating == true && player.status == PlayerStatus.active)
            {
                string response = null;
                while (int.TryParse(response, out score) == false)
                {
                    Console.Write("OK, what should player " + player.Name + "'s score be?");
                    response = Console.ReadLine();
                }
                return score;
            }
            else
            {
                foreach (Card card in player.cards)
                {
                    string cardId = card.Id; // Adjust: Because the card's id has encapsulated, so add a local variable to get the data. In this way, we can get the value of the card
                    string faceValue = cardId.Remove(cardId.Length - 1);
                    switch (faceValue)
                    {
                        case "K":
                        case "Q":
                        case "J":
                            score = score + 10;
                            break;
                        case "A":
                            score = score + 1;
                            break;
                        default:
                            score = score + int.Parse(faceValue);
                            break;
                    }
                }
            }
            return score;
        }

        /// <summary>
        /// Check if the status of any players is active
        /// </summary>
        /// <returns>Return the judgment, if there is an active player, it is true, if not, it is false</returns>
        /// Is called by DoNextTask() method
        private bool CheckActivePlayers()
        {
            // Adjust: When check the first winner, end the game
            foreach (var player in players)
            {
                if (player.status == PlayerStatus.win)
                {
                    return false; // check the first winner
                }
            }

            // Adjust: Returns the end signal if all but one player bust
            int bustPlayerNumber = 0; // Save bust player number

            foreach (var player in players)
            {
                if (player.status == PlayerStatus.bust)
                {
                    bustPlayerNumber++; // If one player bust, plus the bust player number
                }
            }
            // Check if all but one players are bust
            if (bustPlayerNumber == players.Count - 1)
            {
                return false;
            }


            foreach (var player in players)
            {
                if (player.status == PlayerStatus.active)
                {
                    return true; // at least one player is still going!
                }
            }

            return false; // everyone has stayed!
        }


        /// <summary>
        /// Get the winner.
        /// </summary>
        /// <returns>The winner's data</returns>
        /// Is called by DoNextTask() method
        private Player DoFinalScoring()
        {
            int highScore = 0; // Fix: reset this value
            foreach (var player in players)
            {
                cardTable.ShowHand(player);
                if (player.status == PlayerStatus.win) // someone hit 21
                {
                    return player;
                }
                if (player.status == PlayerStatus.stay) // still could win...
                {
                    if (player.score > highScore)
                    {
                        highScore = player.score;
                    }
                }
                // if busted don't bother checking!
            }

            if (highScore > 0) // someone scored, anyway! And no one bust
            {
                // find the FIRST player in list who meets win condition
                return players.Find(player => player.score == highScore); 
            }

            // Fix: Check if there is a player
            if (players.Find(player => player.status == PlayerStatus.bust) != null)
            {
                return players.Find(player => player.status != PlayerStatus.bust);  // Adjust: if all but one player “busts”, remaining player should immediately win
            }

            // No player draws card
            return null;
        }


        /* Implementation:
         * At end of round, each player is asked if they want to keep playing. If a player says no, they are
         * removed from the player list. If only 1 player remains, that player is the winner (equivalent to
         * everyone else “folding” in a card game).  If players choose to keep going, a new deck is built
         * and shuffled. In addition, player list is shuffled, to ensure the same person doesn’t always win 
         * a tiebreaker. 
         */
        /// <summary>
        /// Check if the game can still continue. If it continues, then each player is asked to decide whether to continue with the next round
        /// </summary>
        /// Is called by DoNextTask() method
        private void IsContinue()
        {
            
            int lastTotalPlayers = players.Count;

            // Implementation: Game ends when one player reaches an agreed-upon score (level 2)
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].points > highPoints)
                {
                    highPoints = players[i].points;
                }
            }
            if (highPoints < pointsToGameOver)
            {
                Console.WriteLine("================================");
                for (int i = 0; i < players.Count; i++)
                {
                    Console.Write(players[i].Name + ", do you want to play another round? (Y/N)");
                    string response = Console.ReadLine();
                    // If the player agree, all data of this player will be reset
                    if (response.ToUpper().StartsWith("Y"))
                    {
                        players[i].cards = new List<Card>();
                        players[i].score = 0;
                        players[i].status = PlayerStatus.active;
                    }
                    // If the player disagree, the player will be removed
                    else if (response.ToUpper().StartsWith("N"))
                    {
                        giveUpPlayers.Add(players[i]);
                        players.RemoveAt(i);
                        i--; // When data in the list is removed, the Index of all the data after it is subtracted by one, so here i is also subtracted by one to avoid skipping data. 
                    }
                    else
                    {
                        Console.WriteLine("Please answer Y(es) or N(o)!");
                    }
                }

                // When the number of players decreases, but is greater than one, shuffle player and reset currentPlayer to avoid error. And build a new deck and shuffle it.
                // If players.Count < lastTotalPlayers, it is definitly someone give up
                if (players.Count <= lastTotalPlayers && players.Count > 1) // The number of player minus and there are still two or more players
                {
                    shufflePlayer();
                    deck.buildDeck();
                    deck.Shuffle();
                    currentPlayer = 0;
                    nextTask = Tasks.IntroducePlayers;
                }
                else // Only one player or no player left
                {
                    Console.WriteLine("================================");
                    int winnerPoints = 0;
                    foreach(var player in players)
                    {
                        Console.WriteLine(player.Name + "'s points: " + player.points);
                        if (winnerPoints < player.points)
                        {
                            winnerPoints = player.points;
                        }
                    }
                    foreach(var player in giveUpPlayers)
                    {
                        Console.WriteLine(player.Name + "'s points: " + player.points);
                        if (winnerPoints < player.points)
                        {
                            winnerPoints = player.points;
                        }
                    }

                    if (players.Find(player => player.score == winnerPoints) != null)
                    {
                        Console.WriteLine(players.Find(player => player.score == winnerPoints).Name + " is the final winner!");
                    }

                    if (giveUpPlayers.Find(player => player.score == winnerPoints) != null)
                    {
                        Console.WriteLine(giveUpPlayers.Find(player => player.score == winnerPoints).Name + " is the final winner!");
                    }                  

                    Console.Write("Press <Enter> to exit... ");
                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                    nextTask = Tasks.GameOver;
                }
            }
            else
            {
                Console.WriteLine("================================");
                foreach (var player in players)
                {
                    Console.WriteLine(player.Name + "'s points: " + player.points);
                }
                foreach (var player in giveUpPlayers)
                {
                    Console.WriteLine(player.Name + "'s points: " + player.points);
                }
                Console.WriteLine(players.Find(player => player.points == highPoints).Name + " is the final winner!");
                Console.Write("Press <Enter> to exit... ");
                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                nextTask = Tasks.GameOver;
            } 
        }


        /// <summary>
        /// Shuffle player list
        /// </summary>
        /// Is called by DoNextTask() method
        private void shufflePlayer()
        {
            Random rng = new Random();

            for (int i = 0; i < players.Count; i++)
            {
                Player tmp = players[i];
                int swapindex = rng.Next(players.Count);
                players[i] = players[swapindex];
                players[swapindex] = tmp;
            }
        }
    }
}
