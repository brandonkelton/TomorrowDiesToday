using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface IGameService
    {
        GameModel Game { get; }

        IObservable<string> ErrorMessage { get; }
        IObservable<GameModel> ThisGame { get; }

        Task CreateGame();
        void SetGame(GameModel game);
        Task<bool> JoinGame(string gameId);
    }
}
