using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface IGameState
    {
        GameModel Game { get; }

        void SetGame(GameModel game);
    }
}
