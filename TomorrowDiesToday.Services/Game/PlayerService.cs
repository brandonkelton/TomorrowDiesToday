using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;

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
                _gameService.Game.Players.Add(thisPlayer);
                _gameService.Game.PlayerId = thisPlayer.PlayerId;
                _thisPlayerUpdate.OnNext(thisPlayer);
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
            PlayerModel playerModel = new PlayerModel
            {
                GameId = _gameService.Game.GameId,
                PlayerId = playerId,
                PlayerName = _squadService.NamedHenchmenNames[int.Parse(playerId)],
                Squads = new List<SquadModel>
                {
                    {string.Format("{0}-1", playerId), new SquadModel() },
                    {string.Format("{0}-2", playerId), new SquadModel() },
                    {string.Format("{0}-3", playerId), new SquadModel() },
                    {string.Format("{0}-4", playerId), new SquadModel() },
                    {string.Format("{0}-5", playerId), new SquadModel() },
                    {string.Format("{0}-6", playerId), new SquadModel() },
                }
            };

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
                var playerId = playerModel.PlayerId;
                var player = _players.Where(player => player.PlayerId == playerId).First();
                player = playerModel;
                _otherPlayersUpdate.OnNext(_players.Where(player => player.PlayerId != _playerId).ToList<PlayerModel>());
            });

            _playerUpdateListSubscription = _playerDataService.DataListReceived.Subscribe(playerModels =>
            {
                var newOtherPlayers = playerModels.Where(player => player.PlayerId != _playerId).ToList<PlayerModel>();
                foreach(PlayerModel newOtherPlayer in newOtherPlayers)
                {
                    newOtherPlayer.PlayerName = ((ArmamentType) int.Parse(newOtherPlayer.PlayerId)).ToDescription();
                    var otherPlayer = _players.Where(oldPlayer => oldPlayer.PlayerId == newOtherPlayer.PlayerId).First();
                    otherPlayer = newOtherPlayer;
                }
                _otherPlayersUpdate.OnNext(newOtherPlayers);
            });

            _squadUpdateSubscription = _squadService.SquadUpdate.Subscribe(squadModel =>
            {
                var thisPlayer = _players.Where(player => player.PlayerId == _playerId).First();
                var thisSquad = thisPlayer.Squads.Where(squad => squad.SquadId == squadModel.SquadId).First();
                thisSquad = squadModel;
                _thisPlayerUpdate.OnNext(thisPlayer);
            });
        }

        #endregion
    }
}
