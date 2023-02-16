using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceTo21
{
    public class Card
    {
        private string id; // To store Card's Id
        private string fullName; // To store Card's full name

        public string Id { get { return id; } } // Encapsulation
        public string FullName { get { return fullName; } } // Encapsulation

        public Card (string shortName,string longName)
        {
            id = shortName;
            fullName = longName;
        }

    }
}
