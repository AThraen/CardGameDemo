using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameLib
{
    /// <summary>
    /// The state a game can exist in
    /// </summary>
    public enum GameState
    {
        WaitingToStart,
        InProgress,
        GameOver
    }


    /// <summary>
    /// Actions a player can take
    /// </summary>
    public enum PlayerAction
    {
        TakeFromDeck,
        TakeFromTable,
        Knock
    }


    /// <summary>
    /// Card suits
    /// </summary>
    public enum Suits
    {
        Spades,
        Hearts,
        Clubs,
        Diamonds
    }
}
