using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CardGameLib
{
    public static class CardListExtensions
    {

        public static int CalculateScore(this IEnumerable<Card> Cards)
        {
            return Cards.GroupBy(c => c.Suit).OrderByDescending(grp => grp.Sum(c => c.Value)).First().Sum(c => c.Value);
        }
        
    }
}
