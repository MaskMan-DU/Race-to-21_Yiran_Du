using System;
using System.Collections.Generic;
using System.Linq; // currently only needed if we use alternate shuffle method

namespace RaceTo21
{
    public class Deck
    {
        private List<Card> cards = new List<Card>(); // To store a deck of cards
        private Dictionary<string, string> cardImages = new Dictionary<string, string>(); // Adjust: Create a Dictionary that associates each card “ID” with one of the card image file names (as a String)

        public Deck()
        {          
        }

        // Add a new method, this method will be used build a new deck
        /// <summary>
        /// Re-create a deck of cards
        /// </summary>
        /// Is called by Game object
        public void buildDeck()
        {
            Console.WriteLine("*********** Building deck...");
            cards = new List<Card>();
            cardImages = new Dictionary<string, string>();
            string[] suits = { "Spades", "Hearts", "Clubs", "Diamands" };

            for (int cardVal = 1; cardVal <= 13; cardVal++)
            {
                foreach (string cardSuit in suits)
                {
                    string cardName;
                    string cardLongName;
                    string cardImageName; // Adjust: Add a variable to store image name of the card
                    switch (cardVal)
                    {
                        case 1:
                            cardName = "A";
                            cardLongName = "Ace";
                            cardImageName = "A";
                            break;
                        case 11:
                            cardName = "J";
                            cardLongName = "Jack";
                            cardImageName = "J";
                            break;
                        case 12:
                            cardName = "Q";
                            cardLongName = "Queen";
                            cardImageName = "Q";
                            break;
                        case 13:
                            cardName = "K";
                            cardLongName = "King";
                            cardImageName = "K";
                            break;
                        default:
                            cardName = cardVal.ToString();
                            cardLongName = cardName;
                            cardImageName = cardVal.ToString().PadLeft(2, '0');
                            break;
                    }
                    cards.Add(new Card((cardName + cardSuit.First<char>()), (cardLongName + " of " + cardSuit)));
                    cardImages.Add(cardName + cardSuit.First<char>(), "card_" + cardSuit.ToLower() + "_" + cardImageName + ".png"); // Add 52 dictionaries
                }
            }
        }

        /// <summary>
        /// To disrupt the order of the cards in the deck
        /// </summary>
        /// Is called by Game object
        public void Shuffle()
        {
            Console.WriteLine("Shuffling Cards...");

            Random rng = new Random();

            // one-line method that uses Linq:
            // cards = cards.OrderBy(a => rng.Next()).ToList();

            // multi-line method that uses Array notation on a list!
            // (this should be easier to understand)
            for (int i=0; i<cards.Count; i++)
            {
                Card tmp = cards[i];
                int swapindex = rng.Next(cards.Count);
                cards[i] = cards[swapindex];
                cards[swapindex] = tmp;
            }
        }

        /* Maybe we can make a variation on this that's more useful,
         * but at the moment it's just really to confirm that our 
         * shuffling method(s) worked! And normally we want our card 
         * table to do all of the displaying, don't we?!
         */

        public void ShowAllCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Console.Write(i + ":" + cards[i].Id); // a list property can look like an Array!
                if (i < cards.Count - 1)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.WriteLine("");
                }
            }
        }


        /// <summary>
        /// Get the data of the last card in the deck list, i.e. draw a card from the top of the deck
        /// </summary>
        /// <returns>The card's data, its id and full name</returns>
        /// Is called by Game object
        public Card DealTopCard()
        {
            Card card = new Card( cards[cards.Count - 1].Id, cards[cards.Count - 1].FullName );
            cards.RemoveAt(cards.Count - 1);
            // Console.WriteLine("I'm giving you " + card);
            return card;
        }
    }
}

