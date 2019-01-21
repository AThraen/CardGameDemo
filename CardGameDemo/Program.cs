using CardGameLib;
using System;

namespace CardGameDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let's play 31!");
            Random r = new Random();
            Game G = new Game(r,new ComputerPlayer(r,"Computer"), new ConsolePlayer("You"));
            G.Play();
            Console.ReadLine();
        }
    }
}
