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
        IObservable<PlayerModel> ThisPlayer { get; }
        IObservable<Dictionary<string, TileModel>> Tiles { get; }

        GameModel ThisGame { get; set; }

        Task ChoosePlayer(string playerId);

        Task CreateGame();

        void FlipTile(TileModel tileModel);

        string GenerateGameId();

        Task JoinGame(string gameId);

        Task RequestPlayerUpdate(PlayerModel playerModel);

        Task RequestPlayersUpdate();

        Task RequestTilesUpdate();

        Task SendThisPlayer();

        Task SendTiles();

        void ToggleSquad(SquadModel squadModel);

        void UpdateSquad(SquadModel squadModel);
    }
}
