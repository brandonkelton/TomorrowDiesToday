using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Models.Enums;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface IPlayerService
    {
        IObservable<string> ErrorMessage { get; }
        IObservable<List<PlayerModel>> OtherPlayersUpdate { get; }
        IObservable<PlayerModel> ThisPlayerUpdate { get; }

        Task<bool> ChoosePlayer(ArmamentType playerId);
        Task RequestPlayerUpdate(PlayerModel playerModel);
        Task RequestPlayersUpdate();
        Task SendThisPlayer();
        void PushCurrentPlayer();
    }
}
