﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace CardGameLib
{

        public abstract class Player
    {
        public List<Card> Hand { get; set; }
        public string Name { get; set; }

        public bool HasKnocked { get; set; }

        public abstract void Turn(Game g);

        public string LastAction { get; set; }

        public Player()
        {
            Hand = new List<Card>();
            HasKnocked = false;
        }

        public Player(string Name) : this()
        {
            this.Name = Name;
        }

        public void DrawFromDeck(Game g)
        {
            Hand.Add(g.Deck.DrawCard());
        }

        public void DrawFromTable(Game g)
        {
            Hand.Add(g.Table[g.Table.Count - 1]);
            g.Table.RemoveAt(g.Table.Count - 1);
        }

        public void DropCard(Game g,int idx)
        {
            g.Table.Add(Hand[idx]);
            Hand.RemoveAt(idx);
        }
    }


   
}
