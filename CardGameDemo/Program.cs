﻿using CardGameLib;
using System;

namespace CardGameDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let's play 31!");
            Random r = new Random();
            ComputerPlayer cp = new ComputerPlayer( "Computer");
            cp.Done += Cp_Done;
            Game G = new Game(r,cp, new ConsolePlayer("You"));
            bool isGameOver = false;
            while (!isGameOver)
            {
                Console.WriteLine($"{G.Players[G.CurrentTurn].Name} turn!");
                isGameOver = G.NextTurn();
            }
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine($"--- GAME OVER, {G.Winner.Name} WON WITH {G.Winner.Hand.ToListString()} ---");
            Console.ReadLine();

        }

        private static void Cp_Done(Card obj)
        {
            Console.WriteLine("Computer Player done with their turn, dropped " + obj.ToString());
        }
    }
}
