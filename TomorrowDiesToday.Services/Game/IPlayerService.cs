using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface IPlayerService
    {
        IObservable<string> ErrorMessage { get; }

        IObservable<Dictionary<string, PlayerModel>> OtherPlayersUpdate { get; }

        IObservable<PlayerModel> ThisPlayerUpdate { get; }

        Dictionary<int, string> NameLookup { get; }

        Task<bool> ChoosePlayer(string playerId);

        Task RequestPlayerUpdate(PlayerModel playerModel);

        Task RequestPlayersUpdate();

        Task SendThisPlayer();
    }
}
