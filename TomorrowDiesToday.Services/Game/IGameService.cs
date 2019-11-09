using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface IGameService
    {
        IObservable<string> GameErrorMessage { get; }
        
        IObservable<GameModel> ThisGame { get; }

        GameModel Game { get; }

        Task CreateGame();

        Task<bool> JoinGame(string gameId);
    }
}
