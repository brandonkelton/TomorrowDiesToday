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
        private IGameService _gameService;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;
        private ISquadService _squadService;

        // Subscriptions
        private IDisposable _playerUpdateSubscription = null;
        private IDisposable _playerUpdateListSubscription = null;
        private IDisposable _squadUpdateSubscription = null;

        // Miscellaneous
        private string _gameId => _gameService.Game.GameId;
        private string _playerId => _gameService.Game.PlayerId;
        private List<PlayerModel> _players => _gameService.Game.Players;
        #endregion

        #region Constructor
        public PlayerService(IDataService<PlayerModel, PlayerRequest> playerDataService, IGameService gameService, ISquadService squadService)
        {
            _gameService = gameService;
            _playerDataService = playerDataService;
            _squadService = squadService;
            SubscribeToUpdates();
        }
        #endregion

        #region Public Methods
        public async Task<bool> ChoosePlayer(string playerId)
        {
            PlayerRequest request = new PlayerRequest
            {
                GameId = _gameService.Game.GameId,
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
            var gameId = _gameService.Game.GameId;
            PlayerRequest playerRequest = new PlayerRequest { GameId = gameId };
            await _playerDataService.RequestUpdate(playerRequest);
        }

        public async Task SendThisPlayer()
        {
            var playerId = _gameService.Game.PlayerId;
            var thisPlayer = _gameService.Game.Players.Where(player => player.PlayerId == playerId).FirstOrDefault();
            await _playerDataService.Update(thisPlayer);
        }
        #endregion

        #region Helper Methods
        private void Dispose()
        {
            if (_playerUpdateSubscription != null) _playerUpdateSubscription.Dispose();
            if (_playerUpdateListSubscription != null) _playerUpdateListSubscription.Dispose();
            if (_squadUpdateSubscription != null) _squadUpdateSubscription.Dispose();
        }

        private PlayerModel GeneratePlayer(string playerId)
        {
            ArmamentType playerArmamentType = ((ArmamentType)int.Parse(playerId));
            Armament playerArmament = new Armament(playerArmamentType);
            playerArmament.SetCount(1);
            switch (playerArmamentType)
            {
                case ArmamentType.GeneralGoodman:
                    playerArmament.Stats = new ArmamentStats(2,2,2,2);
                    break;
                case ArmamentType.ArchibaldKluge:
                    playerArmament.Stats = new ArmamentStats(0,1,3,1);
                    break;
                case ArmamentType.AxleRobbins:
                    playerArmament.Stats = new ArmamentStats(1,0,2,2);
                    break;
                case ArmamentType.AzuraBadeau:
                    playerArmament.Stats = new ArmamentStats(2,2,1,0);
                    break;
                case ArmamentType.BorisMyasneek:
                    playerArmament.Stats = new ArmamentStats(3,1,1,0);
                    break;
                case ArmamentType.CassandraOShea:
                    playerArmament.Stats = new ArmamentStats(0,0,2,3);
                    break;
                case ArmamentType.EmmersonBarlow:
                    playerArmament.Stats = new ArmamentStats(1,3,1,0);
                    break;
                case ArmamentType.JinFeng:
                    playerArmament.Stats = new ArmamentStats(0,3,1,1);
                    break;
                case ArmamentType.TheNode:
                    playerArmament.Stats = new ArmamentStats(0,2,2,1);
                    break;
                case ArmamentType.UgoDottore:
                    playerArmament.Stats = new ArmamentStats(1,0,3,1);
                    break;
            }
            PlayerModel playerModel = new PlayerModel
            {
                GameId = _gameService.Game.GameId,
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

            _gameService.Game.Players.Add(playerModel);
            _gameService.Game.PlayerId = playerId;

            // Add chosen Named Henchman to first squad
            var firstSquad = playerModel.Squads.Where(squad => squad.SquadId == string.Format("{0}-1", playerId)).FirstOrDefault();
            firstSquad.Armaments.Add(playerArmament);

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
                        _gameService.Game.Players.Add(otherPlayer);
                    }
                }
                _otherPlayersUpdate.OnNext(_players.Where(player => player.PlayerId != _playerId).ToList());
            });

            _squadUpdateSubscription = _squadService.SquadUpdate.Subscribe(squadModel =>
            {
                var thisPlayer = _players.Where(player => player.PlayerId == _playerId).FirstOrDefault();
                var thisSquad = thisPlayer.Squads.Where(squad => squad.SquadId == squadModel.SquadId).FirstOrDefault();
                thisPlayer.Squads.Remove(thisSquad);
                thisPlayer.Squads.Add(squadModel);
                _thisPlayerUpdate.OnNext(thisPlayer);
            });
        }

        #endregion
    }
}
