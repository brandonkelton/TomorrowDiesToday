using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Game
{
    public class PlayerService : IPlayerService
    {
        public IObservable<string> ErrorMessage => _errorMessage;
        public IObservable<Dictionary<string, PlayerModel>> OtherPlayersUpdate => _otherPlayersUpdate;
        public IObservable<PlayerModel> ThisPlayerUpdate => _thisPlayerUpdate;

        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        private readonly ReplaySubject<Dictionary<string, PlayerModel>> _otherPlayersUpdate = new ReplaySubject<Dictionary<string, PlayerModel>>(1);
        private readonly ReplaySubject<PlayerModel> _thisPlayerUpdate = new ReplaySubject<PlayerModel>(1);

        private IGameService _gameService;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;
        private ISquadService _squadService;

        private IDisposable _playerUpdateSubscription = null;
        private IDisposable _playerUpdateDictSubscription = null;
        private IDisposable _squadUpdateSubscription = null;

        public PlayerService(IDataService<PlayerModel, PlayerRequest> playerDataService, IGameService gameService, ISquadService squadService)
        {
            _gameService = gameService;
            _playerDataService = playerDataService;
            _squadService = squadService;
            SubscribeToUpdates();
        }

        public async Task<bool> ChoosePlayer(string playerName)
        {
            string playerId = _squadService.NamedHenchmenStats[playerName][0].ToString();
            PlayerRequest request = new PlayerRequest
            {
                GameId = _gameService.Game.GameId,
                PlayerId = playerId
            };
            if (!await _playerDataService.Exists(request))
            {
                await _playerDataService.Create(request);
                PlayerModel thisPlayer = GeneratePlayer(playerId);
                _gameService.Game.ThisPlayer = thisPlayer;
                UpdateThisPlayer();
                return true;
            }
            else
            {
                _errorMessage.OnNext("Choose again!");
                return false;
            }
        }

        public async Task RequestPlayerUpdate(PlayerModel playerModel)
        {
            PlayerRequest playerRequest = new PlayerRequest { PlayerId = playerModel.PlayerId };
            await _playerDataService.RequestUpdate(playerRequest);
        }

        public async Task RequestPlayersUpdate()
        {
            var gameId = _gameService.Game.GameId;
            PlayerRequest playerRequest = new PlayerRequest { GameId = gameId };
            await _playerDataService.RequestUpdate(playerRequest);
        }

        public async Task SendThisPlayer()
        {
            await _playerDataService.Update(_gameService.Game.ThisPlayer);
        }

        #region Helper Methods
        private void Dispose()
        {
            if (_playerUpdateSubscription != null) _playerUpdateSubscription.Dispose();
            if (_playerUpdateDictSubscription != null) _playerUpdateDictSubscription.Dispose();
            if (_squadUpdateSubscription != null) _squadUpdateSubscription.Dispose();
        }

        private PlayerModel GeneratePlayer(string playerId)
        {
            PlayerModel playerModel = new PlayerModel
            {
                GameId = _gameService.Game.GameId,
                PlayerId = playerId,
                PlayerName = _squadService.NamedHenchmenNames[int.Parse(playerId)],
                Squads = new Dictionary<string, SquadModel>
                {
                    {string.Format("{0}-1", playerId), new SquadModel() },
                    {string.Format("{0}-2", playerId), new SquadModel() },
                    {string.Format("{0}-3", playerId), new SquadModel() },
                    {string.Format("{0}-4", playerId), new SquadModel() },
                    {string.Format("{0}-5", playerId), new SquadModel() },
                    {string.Format("{0}-6", playerId), new SquadModel() },
                }
            };

            foreach (KeyValuePair<string, SquadModel> squad in playerModel.Squads)
            {
                squad.Value.PlayerId = playerId;
                squad.Value.SquadId = squad.Key;
            }

            // Add chosen Named Henchman to squad 1
            string firstSquad = string.Format("{0}-1", playerId);
            playerModel.Squads[firstSquad].Data["Named Henchman"] = int.Parse(playerId);

            // Calculate stats for squad 1
            _squadService.CalculateSquadStats(playerModel.Squads[firstSquad]);

            return playerModel;
        }

        private void SubscribeToUpdates()
        {
            _playerUpdateSubscription = _playerDataService.DataReceived.Subscribe(playerModel =>
            {
                var otherPlayers = _gameService.Game.OtherPlayers;
                otherPlayers[playerModel.PlayerId] = playerModel;
                _otherPlayersUpdate.OnNext(otherPlayers);
            });

            _playerUpdateDictSubscription = _playerDataService.DataDictReceived.Subscribe(playerModels =>
            {
                foreach(KeyValuePair<string, PlayerModel> player in playerModels)
                {
                    player.Value.PlayerName = _squadService.NamedHenchmenNames[int.Parse(player.Value.PlayerId)];
                }
                _gameService.Game.OtherPlayers = playerModels;
                _otherPlayersUpdate.OnNext(playerModels);
            });

            _squadUpdateSubscription = _squadService.SquadUpdate.Subscribe(squadModel =>
            {
                if (squadModel.PlayerId == _gameService.Game.ThisPlayer.PlayerId)
                {
                    var thisPlayer = _gameService.Game.ThisPlayer;
                    _thisPlayerUpdate.OnNext(thisPlayer);
                }
                else
                {
                    var otherPlayers = _gameService.Game.OtherPlayers;
                    _otherPlayersUpdate.OnNext(otherPlayers);
                }
            });
        }

        private void UpdateThisPlayer()
        {
            _thisPlayerUpdate.OnNext(_gameService.Game.ThisPlayer);
        }
        #endregion
    }
}
