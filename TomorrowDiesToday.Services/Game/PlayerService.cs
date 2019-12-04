using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using TomorrowDiesToday.Models.Enums;
using TomorrowDiesToday.Services.LocalStorage;

namespace TomorrowDiesToday.Services.Game
{
    public class PlayerService : IPlayerService
    {
        #region Properties
        // Observables
        public IObservable<string> ErrorMessage => _errorMessage;
        public IObservable<List<PlayerModel>> OtherPlayersUpdate => _otherPlayersUpdate;
        public IObservable<PlayerModel> ThisPlayerUpdate => _thisPlayerUpdate;

        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        private readonly ReplaySubject<List<PlayerModel>> _otherPlayersUpdate = new ReplaySubject<List<PlayerModel>>(1);
        private readonly ReplaySubject<PlayerModel> _thisPlayerUpdate = new ReplaySubject<PlayerModel>(1);

        // Requred Service(s)
        private IGameState _gameState;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;
        private ISquadService _squadService;

        // Subscriptions
        private IDisposable _playerUpdateSubscription = null;
        private IDisposable _playerUpdateListSubscription = null;
        private IDisposable _squadUpdateSubscription = null;

        // Miscellaneous
        private string _gameId => _gameState.Game.GameId;
        private ArmamentType _playerId => _gameState.Game.PlayerId;
        private List<PlayerModel> _players => _gameState.Game.Players;
        private List<PlayerModel> _otherPlayers => _players.Where(player => player.PlayerId != _playerId).ToList();

        #endregion

        #region Constructor
        public PlayerService(IDataService<PlayerModel, PlayerRequest> playerDataService, IGameState gameState, ISquadService squadService)
        {
            _gameState = gameState;
            _playerDataService = playerDataService;
            _squadService = squadService;

            SubscribeToUpdates();
        }
        #endregion

        #region Public Methods
        public async Task<bool> ChoosePlayer(ArmamentType playerId)
        {
            PlayerRequest request = new PlayerRequest
            {
                GameId = _gameState.Game.GameId,
                PlayerId = playerId
            };
            if (!await _playerDataService.Exists(request))
            {
                var playerModel = GeneratePlayer(playerId);
                await _playerDataService.Create(playerModel);
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
            var gameId = _gameState.Game.GameId;
            PlayerRequest playerRequest = new PlayerRequest { GameId = gameId };
            await _playerDataService.RequestUpdate(playerRequest);
        }

        public async Task SendThisPlayer()
        {
            var playerId = _gameState.Game.PlayerId;
            var thisPlayer = _gameState.Game.Players.Where(player => player.PlayerId == playerId).FirstOrDefault();

            var request = new PlayerRequest { GameId = _gameState.Game.GameId, PlayerId = playerId };
            if (!await _playerDataService.Exists(request))
            {
                await _playerDataService.Create(thisPlayer);
            } else
            {
                await _playerDataService.Update(thisPlayer);
            }
        }

        public void PushCurrentPlayer()
        {
            var playerId = _gameState.Game.PlayerId;
            var thisPlayer = _gameState.Game.Players.Where(player => player.PlayerId == playerId).FirstOrDefault();

            _thisPlayerUpdate.OnNext(thisPlayer);
        }
        #endregion

        #region Helper Methods
        private void Dispose()
        {
            if (_playerUpdateSubscription != null) _playerUpdateSubscription.Dispose();
            if (_playerUpdateListSubscription != null) _playerUpdateListSubscription.Dispose();
            if (_squadUpdateSubscription != null) _squadUpdateSubscription.Dispose();
        }

        private PlayerModel GeneratePlayer(ArmamentType playerId)
        {
            PlayerModel playerModel = new PlayerModel
            {
                GameId = _gameState.Game.GameId,
                PlayerId = playerId,
                Squads = new List<SquadModel>
                {
                    new SquadModel
                    {
                        PlayerId = playerId,
                        SquadId = string.Format("{0}-1", playerId),
                    },
                    new SquadModel
                    {
                        PlayerId = playerId,
                        SquadId = string.Format("{0}-2", playerId),
                    },
                    new SquadModel
                    {
                        PlayerId = playerId,
                        SquadId = string.Format("{0}-3", playerId),
                    }
                }
            };

            foreach(SquadModel squad in playerModel.Squads)
            {
                ArmamentStats stats = new ArmamentStats();
                switch (playerId)
                {
                    case ArmamentType.GeneralGoodman:
                        stats = new ArmamentStats(2, 2, 2, 2);
                        break;
                    case ArmamentType.ArchibaldKluge:
                        stats = new ArmamentStats(0, 1, 3, 1);
                        break;
                    case ArmamentType.AxleRobbins:
                        stats = new ArmamentStats(1, 0, 2, 2);
                        break;
                    case ArmamentType.AzuraBadeau:
                        stats = new ArmamentStats(2, 2, 1, 0);
                        break;
                    case ArmamentType.BorisMyasneek:
                        stats = new ArmamentStats(3, 1, 1, 0);
                        break;
                    case ArmamentType.CassandraOShea:
                        stats = new ArmamentStats(0, 0, 2, 3);
                        break;
                    case ArmamentType.EmmersonBarlow:
                        stats = new ArmamentStats(1, 3, 1, 0);
                        break;
                    case ArmamentType.JinFeng:
                        stats = new ArmamentStats(0, 3, 1, 1);
                        break;
                    case ArmamentType.TheNode:
                        stats = new ArmamentStats(0, 2, 2, 1);
                        break;
                    case ArmamentType.UgoDottore:
                        stats = new ArmamentStats(1, 0, 3, 1);
                        break;
                }
                squad.Armaments.Add(new Armament(playerId, stats));
            }

            _gameState.Game.Players.Add(playerModel);
            _gameState.Game.PlayerId = playerId;;

            // Add chosen Named Henchman to first squad
            var firstSquad = playerModel.Squads.Where(squad => squad.SquadId == string.Format("{0}-1", playerId)).FirstOrDefault();
            var playerArmament = firstSquad.Armaments.Where(armament => armament.ArmamentType == playerId).FirstOrDefault();
            playerArmament.SetCount(1);

            // Calculate stats for first squad
            _squadService.CalculateSquadStats(firstSquad);

            _thisPlayerUpdate.OnNext(playerModel); ;

            return playerModel;
        }

        private void SubscribeToUpdates()
        {
            _playerUpdateSubscription = _playerDataService.DataReceived.Subscribe(playerModel =>
            {
                var playerId = playerModel.PlayerId;
                var existingPlayer = _players.Where(player => player.PlayerId == playerId).FirstOrDefault();
                if (existingPlayer != null)
                {
                    existingPlayer.Squads = playerModel.Squads;
                }
            
                _otherPlayersUpdate.OnNext(_players.Where(player => player.PlayerId != _playerId).ToList());
            });

            _playerUpdateListSubscription = _playerDataService.DataListReceived.Subscribe(playerModels =>
            {
                var otherPlayers = playerModels.Where(player => player.PlayerId != _playerId).ToList();
                foreach(PlayerModel otherPlayer in otherPlayers)
                {
                    var existingPlayer = _players.Where(player => player.PlayerId == otherPlayer.PlayerId).FirstOrDefault();
                    if (existingPlayer != null)
                    {
                        existingPlayer.Squads = otherPlayer.Squads;
                    }
                    else
                    {
                        _gameState.Game.Players.Add(otherPlayer);
                    }
                }
                _otherPlayersUpdate.OnNext(_players.Where(player => player.PlayerId != _playerId).ToList());
            });

            _squadUpdateSubscription = _squadService.SquadUpdate.Subscribe(squadModel =>
            {
                var targetPlayer = _players.Where(player => player.PlayerId == squadModel.PlayerId).FirstOrDefault();
                var targetSquad = targetPlayer.Squads.Where(squad => squad.SquadId == squadModel.SquadId).FirstOrDefault();
                targetPlayer.Squads.Remove(targetSquad);
                targetPlayer.Squads.Add(squadModel);

                if (squadModel.PlayerId == _playerId)
                {
                    _thisPlayerUpdate.OnNext(targetPlayer);
                }

                 _otherPlayersUpdate.OnNext(_otherPlayers);
            });
        }

        #endregion
    }
}
