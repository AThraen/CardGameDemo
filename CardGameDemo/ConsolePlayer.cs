using CardGameLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CardGameDemo
{
    public class ConsolePlayer : Player
    {
        public ConsolePlayer(string Name) : base(Name) { }

        public override void Turn()
        {
            Console.WriteLine("Your turn. Your hand: ");
            foreach (var c in Hand)
            {
                Console.WriteLine("\t" + c.ToString());
            }
            if (Game.Table.Count > 0)
            {
                Console.WriteLine("On the table there is " + Game.Table.First().ToString() + ". Do you want to draw from the Table (T) or the Deck (D)?");
                if (Console.ReadLine().ToUpper() == "T") DrawFromTable();
                else DrawFromDeck();
            }
            else
            {
                DrawFromDeck();
            }
            Console.WriteLine("You drew a card. Your hand: ");
            foreach (var c in Hand)
            {
                Console.WriteLine("\t" + c.ToString());
            }

            Console.WriteLine("Which card to drop? (0-3)");
            string input = Console.ReadLine();
            int action = int.Parse(input);
            DropCard(action);
            Console.WriteLine("Your score: " + Hand.CalculateScore());
        }
    }
}
