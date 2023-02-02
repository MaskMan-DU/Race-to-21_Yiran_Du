using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceTo21
{
    public class Card
    {
        public string id;
        public string fullName;
        public int value;

        public Card (string shortName,string longName, int point)
        {
            id = shortName;
            fullName = longName;
            value = point;
        }

    }
}
