using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public class GameState : IGameState
    {
        public GameModel Game { get; private set; }

        public void SetGame(GameModel game)
        {
            Game = game;
        }
    }
}
