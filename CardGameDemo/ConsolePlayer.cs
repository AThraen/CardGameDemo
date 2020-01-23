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

        public override void Turn(Game g)
        {
            Console.WriteLine("Your turn. Your hand: ");
            foreach (var c in Hand)
            {
                Console.WriteLine("\t" + c.ToString());
            }
            if (g.Table.Count > 0)
            {
                Console.WriteLine("On the table there is " + g.Table.Last().ToString() + ". Do you want to draw from the Table (T) or the Deck (D) or Call/Knock (C)?");
                var c = Console.ReadLine().ToUpper();
                if (c == "T") DrawFromTable(g);
                else if(c=="D") DrawFromDeck(g);
                else
                {
                    this.HasKnocked = true;
                }
            }
            else
            {
                DrawFromDeck(g);
            }
            Console.WriteLine("You drew a card. Your hand: ");
            foreach (var c in Hand)
            {
                Console.WriteLine("\t" + c.ToString());
            }

            Console.WriteLine("Which card to drop? (0-3)");
            string input = Console.ReadLine();
            int action = int.Parse(input);
            DropCard(g,action);
            Console.WriteLine("Your score: " + Hand.CalculateScore());
        }
    }
}
