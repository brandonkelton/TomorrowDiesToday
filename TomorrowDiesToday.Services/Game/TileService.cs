using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Models.Enums;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Game
{
    public class TileService : ITileService
    {
        #region Properties
        // Observables
        public IObservable<List<TileModel>> ActiveTilesUpdate => _activeTilesUpdate;
        public IObservable<List<TileModel>> AllTilesUpdate => _allTilesUpdate;
        public IObservable<string> ErrorMessage => _errorMessage;
        private readonly ReplaySubject<List<TileModel>> _activeTilesUpdate = new ReplaySubject<List<TileModel>>(1);
        private readonly ReplaySubject<List<TileModel>> _allTilesUpdate = new ReplaySubject<List<TileModel>>(1);
        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);

        // Required Service(s)
        private IDataService<GameModel, GameRequest> _gameDataService;
        private IGameService _gameService;
        private ISquadService _squadService;

        // Subscriptions
        private IDisposable _selectedSquadStatsUpdateSubscription = null;
        private IDisposable _tilesUpdateSubscription = null;

        private List<TileModel> _activeTiles => _gameService.Game.Tiles.Where(t => t.IsActive).ToList();
        private List<TileModel> _allTiles => _gameService.Game.Tiles;

        #endregion

        #region Constructor
        public TileService(IDataService<GameModel, GameRequest> gameDataService, IGameService gameService, ISquadService squadService)
        {
            _gameService = gameService;
            _gameDataService = gameDataService;
            _squadService = squadService;
            InitializeAllTiles();
            SubscribeToUpdates();
        }
        #endregion

        #region Public Methods
        
        public void DecrementAlertTokens(TileModel tileModel)
        {
            if (tileModel.AlertTokens - 1 >= 0)
            {
                tileModel.AlertTokens -= 1;

                if (tileModel.IsActive)
                {
                    _activeTilesUpdate.OnNext(_activeTiles);
                }

                _allTilesUpdate.OnNext(_allTiles);
            }
            else
            {
                _errorMessage.OnNext(ErrorType.InvalidAlertTokenCount.ToDescription());
            }
        }

        public void IncrementAlertTokens(TileModel tileModel)
        {
            tileModel.AlertTokens += 1;

            if (tileModel.IsActive)
            {
                _activeTilesUpdate.OnNext(_activeTiles);
            }

            _allTilesUpdate.OnNext(_allTiles);
        }

        public async Task RequestTilesUpdate()
        {
            GameRequest gameRequest = new GameRequest { GameId = _gameService.Game.GameId };

            await _gameDataService.RequestUpdate(gameRequest);
        }

        public async Task SendTiles()
        {
            await _gameDataService.Update(_gameService.Game);
        }

        public void ToggleActive(TileModel tileModel)
        {
            if (tileModel.TileType != TileType.CIABuilding && tileModel.TileType != TileType.InterpolHQ)
            {
                tileModel.IsActive = !tileModel.IsActive;
                var activeTiles = _gameService.Game.Tiles.Where(tile => tile.IsActive).ToList();

                _activeTilesUpdate.OnNext(activeTiles);
                _allTilesUpdate.OnNext(_gameService.Game.Tiles);
            }
            else
            {
                _errorMessage.OnNext(ErrorType.InvalidTileDeactivation.ToDescription());
            }
        }

        public void ToggleAgent(TileModel tileModel)
        {
            if (tileModel.IsDoomsday || tileModel.TileType == TileType.CIABuilding)
            { // Toggle CIA Agent
                var existingAgentTile = _gameService.Game.Tiles.Where(tile => tile.IsAgentCIA).FirstOrDefault();
                if (existingAgentTile != null)
                { // Agent is currently on a tile
                    if (tileModel.Equals(existingAgentTile))
                    { // The Agent is on this tile, so remove it.
                        tileModel.IsAgentCIA = false;
                    }
                    else
                    { // Agent is not on this tile, so remove it from previous tile and add to this tile
                        existingAgentTile.IsAgentCIA = false;
                        tileModel.IsAgentCIA = true;
                    }
                }
                else
                { // No tile contained the CIA Agent, so add it to this tile
                    tileModel.IsAgentCIA = true;
                }
            }
            else
            { // Toggle Interpol Agent
                var existingAgentTile = _gameService.Game.Tiles.Where(tile => tile.IsAgentInterpol).FirstOrDefault();
                if (existingAgentTile != null)
                { // Agent is currently on a tile
                    if (tileModel.Equals(existingAgentTile))
                    { // The Agent is on this tile, so remove it.
                        tileModel.IsAgentInterpol = false;
                    }
                    else
                    { // Agent is not on this tile, so remove it from previous tile and add to this tile
                        existingAgentTile.IsAgentInterpol = false;
                        tileModel.IsAgentInterpol = true;
                    }
                }
                else
                { // No tile contained the CIA Agent, so add it to this tile
                    tileModel.IsAgentInterpol = true;
                }
            }

            if (tileModel.IsActive)
            {
                _activeTilesUpdate.OnNext(_activeTiles);
            }

            _allTilesUpdate.OnNext(_allTiles);
        }

        public void ToggleFlipped(TileModel tileModel)
        {
            if (!tileModel.IsDoomsday && !tileModel.IsHQ)
            {
                tileModel.IsFlipped = !tileModel.IsFlipped;
                if (tileModel.IsActive)
                {
                    var activeTiles = _gameService.Game.Tiles.Where(tile => tile.IsActive).ToList();
                    _activeTilesUpdate.OnNext(activeTiles);
                }
                _allTilesUpdate.OnNext(_gameService.Game.Tiles);
            }
            else
            {
                _errorMessage.OnNext(ErrorType.InvalidTileFlip.ToDescription());
            }
        }

        public void ToggleGlobalSecurityEvent()
        {
            _allTiles.ForEach(t => t.IsGlobalSecurityEvent = !t.IsGlobalSecurityEvent);
            _allTilesUpdate.OnNext(_allTiles);
            _activeTilesUpdate.OnNext(_activeTiles);
        }

        #endregion

        #region Helper Methods
        private void Dispose()
        {
            if (_selectedSquadStatsUpdateSubscription != null) _selectedSquadStatsUpdateSubscription.Dispose();
            if (_tilesUpdateSubscription != null) _tilesUpdateSubscription.Dispose();
        }

        private void InitializeAllTiles()
        {
            List<TileModel> allTiles = new List<TileModel>();

            allTiles.Add(new TileModel(TileType.BloodDiamondHarvest, new TileStats(3,2,1,0), new TileStats(3,2,2,0)));
            allTiles.Add(new TileModel(TileType.SkinTrade, new TileStats(1,1,2,2), new TileStats(1,1,2,3)));
            allTiles.Add(new TileModel(TileType.SocialEngineeringScams, new TileStats(0,1,3,2), new TileStats(0,1,3,1)));
            allTiles.Add(new TileModel(TileType.CounterfeitingOperation, new TileStats(0,3,2,1), new TileStats(0,3,2,0)));
            allTiles.Add(new TileModel(TileType.PonziSchemes, new TileStats(0,1,2,3), new TileStats(0,1,3,2)));
            allTiles.Add(new TileModel(TileType.PoliticalCorruption, new TileStats(0,2,2,2), new TileStats(0,1,2,2)));
            allTiles.Add(new TileModel(TileType.HackerCell, new TileStats(0,3,3,0), new TileStats(0,3,4,0)));
            allTiles.Add(new TileModel(TileType.ArtThievery, new TileStats(1,2,2,1), new TileStats(0,3,3,1)));
            allTiles.Add(new TileModel(TileType.ExoticCarGTA, new TileStats(0,2,4,0), new TileStats(0,3,3,0)));
            allTiles.Add(new TileModel(TileType.SmugglingRing, new TileStats(0,4,1,1), new TileStats(0,2,2,2)));
            allTiles.Add(new TileModel(TileType.ArmsDealing, new TileStats(3,1,1,1), new TileStats(1,3,1,0)));
            allTiles.Add(new TileModel(TileType.MaritimePiracy, new TileStats(3,1,2,0), new TileStats(3,2,2,0)));
            allTiles.Add(new TileModel(TileType.RigSportsEvents, new TileStats(0,2,3,1), new TileStats(0,1,2,2)));
            allTiles.Add(new TileModel(TileType.GamblingDens, new TileStats(1,2,3,0), new TileStats(1,1,3,0)));
            allTiles.Add(new TileModel(TileType.NarcoticsDistribution, new TileStats(1,2,1,2), new TileStats(1,3,2,1)));
            allTiles.Add(new TileModel(TileType.CBRNEDealing, new TileStats(1,0,4,1), new TileStats(1,0,4,2)));
            allTiles.Add(new TileModel(TileType.IvoryPoaching, new TileStats(3,0,2,1), new TileStats(3,2,2,0)));
            allTiles.Add(new TileModel(TileType.MurderInc, new TileStats(1,3,1,1), new TileStats(2,4,1,0)));
            allTiles.Add(new TileModel(TileType.CrashWallStreet, new TileStats(0,6,8,1)));
            allTiles.Add(new TileModel(TileType.DestroyIXPs, new TileStats(2,6,6,1))); 
            allTiles.Add(new TileModel(TileType.BringDownSatellites, new TileStats(2,2,7,4)));
            allTiles.Add(new TileModel(TileType.KidnapMilitaryAndPoliticalLeaders, new TileStats(3,6,3,3)));
            allTiles.Add(new TileModel(TileType.DeployNeurotoxin, new TileStats(4,3,6,2)));
            allTiles.Add(new TileModel(TileType.BurgleFortKnox, new TileStats(1,8,4,2)));
            allTiles.Add(new TileModel(TileType.SupplantMajorAfricanWarlords, new TileStats(8,4,1,2)));
            allTiles.Add(new TileModel(TileType.TakeOverSouthAmericanCartels, new TileStats(4,8,1,2)));
            allTiles.Add(new TileModel(TileType.HijackMajorWorldMediaOutlets, new TileStats(0,2,7,6)));
            allTiles.Add(new TileModel(TileType.InfiltrateAsianIntelligenceAgencies, new TileStats(3,4,4,4)));
            allTiles.Add(new TileModel(TileType.CorruptTheUN, new TileStats(0,2,5,8)));
            allTiles.Add(new TileModel(TileType.CIABuilding, new TileStats(2,2,2,2)));
            allTiles.Add(new TileModel(TileType.InterpolHQ, new TileStats(1,2,2,1)));

            _gameService.Game.Tiles = allTiles;
            _allTilesUpdate.OnNext(allTiles);
        }

        private void SubscribeToUpdates()
        {
            _selectedSquadStatsUpdateSubscription = _squadService.SelectedSquadStatsUpdate.Subscribe(squadStats =>
            {
                var activeTiles = _gameService.Game.Tiles.Where(tile => tile.IsActive).ToList();
                activeTiles.ForEach(t => SuccessCheck(t));
                _activeTilesUpdate.OnNext(activeTiles);
                _allTilesUpdate.OnNext(_gameService.Game.Tiles);
            });

            _tilesUpdateSubscription = _gameDataService.DataReceived.Subscribe(gameModel =>
            {
                var receivedTiles = gameModel.Tiles;
                _gameService.Game.Tiles.ForEach(tile =>
                {
                    var receivedTile = receivedTiles.Where(t => t.TileId == tile.TileId).FirstOrDefault();
                    tile.AlertTokens = receivedTile.AlertTokens;
                    tile.IsActive = receivedTile.IsActive;
                    tile.IsAgentCIA = receivedTile.IsAgentCIA;
                    tile.IsAgentInterpol = receivedTile.IsAgentInterpol;
                    tile.IsDoomsday = receivedTile.IsDoomsday;
                    tile.IsFlipped = receivedTile.IsFlipped;
                    tile.IsGlobalSecurityEvent = receivedTile.IsGlobalSecurityEvent;
                    tile.IsHQ = receivedTile.IsHQ;
                });
                _allTilesUpdate.OnNext(_allTiles);
            });
        }

        private void SuccessCheck(TileModel tileModel)
        {
            TileStats tileStats = tileModel.Stats;
            SquadStats selectedSquadStats = _gameService.Game.SelectedSquadStats;
            tileModel.Success = true;

            if (tileStats.Combat.Value > selectedSquadStats.Combat.Value)
            {
                tileModel.Success = false;
            }
            else if (tileStats.Stealth.Value > selectedSquadStats.Stealth.Value)
            {
                tileModel.Success = false;
            }
            else if (tileStats.Cunning.Value > selectedSquadStats.Cunning.Value)
            {
                tileModel.Success = false;
            }
            else if (tileStats.Diplomacy.Value > selectedSquadStats.Diplomacy.Value)
            {
                tileModel.Success = false;
            }
        }

        #endregion
    }
}
