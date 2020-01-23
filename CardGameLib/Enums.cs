using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameLib
{
    public enum GameState
    {
        WaitingToStart,
        InProgress,
        GameOver
    }

    public enum PlayerAction
    {
        TakeFromDeck,
        TakeFromTable,
        Knock
    }
}
