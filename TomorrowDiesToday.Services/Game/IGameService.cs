using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface IGameService
    {
        IObservable<string> ErrorMessage { get; }
        IObservable<GameModel> ThisGame { get; }

        Task CreateGame();
        Task SendGame();
        void PushGame();
        Task<bool> JoinGame(string gameId);
        Task RequestGameUpdate();
    }
}
