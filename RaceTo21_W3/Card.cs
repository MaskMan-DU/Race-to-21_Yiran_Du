using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceTo21
{
    public class Card
    {
        private string id;
        private string fullName;

        public string Id { get { return id; } }
        public string FullName { get { return fullName; } }

        public Card (string shortName,string longName)
        {
            id = shortName;
            fullName = longName;
        }

    }
}
