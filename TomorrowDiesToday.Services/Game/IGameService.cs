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
        IObservable<Dictionary<string, PlayerModel>> OtherPlayers { get; }
        IObservable<GameModel> ThisGame { get; }
        IObservable<PlayerModel> ThisPlayer { get; }
        IObservable<Dictionary<string, TileModel>> Tiles { get; }

        Task<bool> ChoosePlayer(string playerId);

        Task CreateGame();

        Task<bool> JoinGame(string gameId);

        Task RequestPlayerUpdate(PlayerModel playerModel);

        Task RequestPlayersUpdate();

        Task RequestTilesUpdate();

        Task SendThisPlayer();

        Task SendTiles();

        void ToggleSquad(SquadModel squadModel);

        void ToggleTile(TileModel tileModel);

        void UpdateSquad(SquadModel squadModel);

        void UpdateTile(TileModel tileModel);
    }
}
