using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CardGameLib
{
    /// <summary>
    /// Extension methods to list of cards
    /// </summary>
    public static class CardListExtensions
    {

        /// <summary>
        /// Calculate score for a list of cards
        /// </summary>
        /// <param name="Cards"></param>
        /// <returns></returns>
        public static int CalculateScore(this IEnumerable<Card> Cards)
        {
            return Cards.GroupBy(c => c.Suit).OrderByDescending(grp => grp.Sum(c => c.Value)).First().Sum(c => c.Value);
        }
        
        public static string ToListString(this IEnumerable<Card> Cards)
        {
            return string.Join(",", Cards);
        }
    }
}
