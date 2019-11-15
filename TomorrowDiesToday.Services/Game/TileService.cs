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

        // Subscriptions
        private IDisposable _tilesUpdateSubscription = null;

        #endregion

        #region Constructor
        public TileService(IDataService<GameModel, GameRequest> gameDataService, IGameService gameService)
        {
            _gameService = gameService;
            _gameDataService = gameDataService;
            InitializeAllTiles();
            SubscribeToUpdates();
        }
        #endregion

        #region Public Methods
        public async Task RequestActiveTilesUpdate()
        {
            GameRequest gameRequest = new GameRequest { GameId = _gameService.Game.GameId };
            await _gameDataService.RequestUpdate(gameRequest);
        }

        public async Task SendActiveTiles()
        {
            await _gameDataService.Update(_gameService.Game);
        }

        public void ToggleActive(TileModel tileModel)
        {
            tileModel.IsActive = !tileModel.IsActive;
            var activeTiles = _gameService.Game.Tiles.Where(tile => tile.IsActive).ToList();
            _activeTilesUpdate.OnNext(activeTiles);
            _allTilesUpdate.OnNext(_gameService.Game.Tiles);
        }

        public void ToggleFlipped(TileModel tileModel)
        {
            tileModel.IsFlipped = !tileModel.IsFlipped;
            _allTilesUpdate.OnNext(_gameService.Game.Tiles);
        }

        #endregion

        #region Helper Methods
        private void Dispose()
        {
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

        //private Dictionary<string, int> HandleTileAlerts(int alerts, Dictionary<string, int> tileDictionary)
        //{

        //    if (alerts > 0)
        //    {
        //        tileDictionary["Combat"] *= alerts;
        //        tileDictionary["Stealth"] *= alerts;
        //        tileDictionary["Cunning"] *= alerts;
        //        tileDictionary["Diplomacy"] *= alerts;
        //    }

        //    return tileDictionary;
        //}

        //private Dictionary<string, int> ParseToDictionary(string dataStrip)
        //{
        //    string[] keyArray = new string[] { "Combat", "Stealth", "Cunning", "Diplomacy" };
        //    string[] stringArray = dataStrip.Split(',');
        //    int[] statusValueArray = Array.ConvertAll(stringArray, int.Parse);
        //    Dictionary<string, int> tileStatus = new Dictionary<string, int>();

        //    if (keyArray.Length == statusValueArray.Length)
        //    {
        //        for (int i = 0; i < keyArray.Length; i++)
        //        {
        //            tileStatus.Add(keyArray[i], statusValueArray[i]);
        //        }
        //    }

        //    return tileStatus;
        //}

        private void SubscribeToUpdates()
        {
            _tilesUpdateSubscription = _gameDataService.DataReceived.Subscribe(gameModel =>
            {
                var tiles = gameModel.Tiles;
                _gameService.Game.Tiles = tiles;
                _allTilesUpdate.OnNext(tiles);
            });
        }

        //private bool SuccessCheck(Dictionary<string, int> tileStats, Dictionary<string, int> squadStats)
        //{
        //    int combatResult = squadStats["Combat"] - tileStats["Combat"];
        //    int stealthResult = squadStats["Stealth"] - tileStats["Stealth"];
        //    int cunningResult = squadStats["Cunning"] - tileStats["Cunning"];
        //    int diplomacyResult = squadStats["Diplomacy"] - tileStats["Diplomacy"];

        //    if (combatResult >= 0 && stealthResult >= 0 && cunningResult >= 0 && diplomacyResult >= 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private Dictionary<string, int> TileLookup(string tileName, Boolean flipped, int alerts) //returns Dictionary of matching name
        //{
        //    Dictionary<string, int> tileDictionary;
        //    string dataStrip;

        //    if (flipped == true)
        //    {
        //        dataStrip = _flipMissions[tileName];
        //    }
        //    else if (flipped == false)
        //    {
        //        dataStrip = _missions[tileName];
        //    }
        //    else
        //    {
        //        //throw some exception
        //        System.ArgumentException argEx = new System.ArgumentException("Index is out of range", "index");
        //        throw argEx;
        //    }

        //    tileDictionary = ParseToDictionary(dataStrip);
        //    if (alerts > 0)
        //    {
        //        HandleTileAlerts(alerts, tileDictionary);
        //    }

        //    return tileDictionary;
        //}
        #endregion
    }
}
