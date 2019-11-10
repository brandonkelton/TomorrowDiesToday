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
    public class TileService : ITileService
    {
        #region Properties
        // Observables
        public IObservable<Dictionary<string, TileModel>> ActiveTilesUpdate => _activeTilesUpdate;
        public IObservable<Dictionary<string, TileModel>> AllTilesUpdate => _allTilesUpdate;
        public IObservable<string> ErrorMessage => _errorMessage;
        private readonly ReplaySubject<Dictionary<string, TileModel>> _activeTilesUpdate = new ReplaySubject<Dictionary<string, TileModel>>(1);
        private readonly ReplaySubject<Dictionary<string, TileModel>> _allTilesUpdate = new ReplaySubject<Dictionary<string, TileModel>>(1);
        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);

        // Required Service(s)
        private IDataService<GameModel, GameRequest> _gameDataService;
        private IGameService _gameService;

        // Subscriptions
        private IDisposable _tilesUpdateSubscription = null;

        // Miscellaneous
        private Dictionary<string, int[]> _resourceMissions = new Dictionary<string, int[]>
        { //<TileName, TileStats>
            //Resource Missions
            { "Blood Diamond Harvest",    new int[] {  1, 3, 2, 1, 0 } },
            { "Skin Trade",               new int[] {  2, 1, 1, 2, 2 } },
            { "Social Engineering Scams", new int[] {  3, 0, 1, 3, 2 } },
            { "Counterfeiting Operation", new int[] {  4, 0, 3, 2, 1 } },
            { "Ponzi Schemes",            new int[] {  5, 0, 1, 2, 3 } },
            { "Political Corruption",     new int[] {  6, 0, 2, 2, 2 } },
            { "Hacker Cell",              new int[] {  7, 0, 3, 3, 0 } },
            { "Art Thievery",             new int[] {  8, 1, 2, 2, 1 } },
            { "Exotic Car GTA",           new int[] {  9, 0, 2, 4, 0 } },
            { "Smuggling Ring",           new int[] { 10, 0, 4, 1, 1 } },
            { "Arms Dealing",             new int[] { 11, 3, 1, 1, 1 } },
            { "Maritime Piracy",          new int[] { 12, 3, 1, 2, 0 } },
            { "Rig Sports Events",        new int[] { 13, 0, 2, 3, 1 } },
            { "Gambling Dens",            new int[] { 14, 1, 2, 3, 0 } },
            { "Narcotics Distribution",   new int[] { 15, 1, 2, 1, 2 } },
            { "CBRNE Dealing",            new int[] { 16, 1, 0, 4, 1 } },
            { "Ivory Poaching",           new int[] { 17, 3, 0, 2, 1 } },
            { "Murder Inc",               new int[] { 18, 1, 3, 1, 1 } }
        };
        private Dictionary<string, int[]> _doomsdayMissions = new Dictionary<string, int[]>
        { //<TileName, TileStats>
            //Doomsday Missions
            { "Crash Wall Street",                      new int[] { 19, 0, 6, 8, 1 } },
            { "Destroy IXP's",                          new int[] { 20, 2, 6, 6, 1 } },
            { "Bring Down Satellites",                  new int[] { 21, 2, 2, 7, 4 } },
            { "Kidnap Military & Political Leaders",    new int[] { 22, 3, 6, 3, 3 } },
            { "Deploy Neurotoxin",                      new int[] { 23, 4, 3, 6, 2 } },
            { "Acquire Russian Nuclear Stockpile",      new int[] { 24, 6, 5, 4, 0 } },
            { "Burgle Fort Knox",                       new int[] { 25, 1, 8, 4, 2 } },
            { "Supplant Major African Warlords",        new int[] { 26, 8, 4, 1, 2 } },
            { "Take Over South American Cartels",       new int[] { 27, 4, 8, 1, 2 } },
            { "Hijack Major World Media Outlets",       new int[] { 28, 0, 2, 7, 6 } },
            { "Infiltrate Asian Intelligence Agencies", new int[] { 29, 3, 4, 4, 4 } },
            { "Corrupt the U.N.",                       new int[] { 30, 0, 2, 5, 8 } }
        };
        private Dictionary<string, int[]> _agentHeadquarters = new Dictionary<string, int[]>
        { //<TileName, TileStats>
            //Agent Headquarters
            { "CIA Building", new int[] { 31, 2, 2, 2, 2 }},
            { "Interpol HQ",  new int[] { 32, 1, 2, 2, 1 }}
        };
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

        public void ToggleTile(TileModel tileModel)
        {
            tileModel.IsFlipped = !tileModel.IsFlipped;

            var activeTiles = _gameService.Game.ActiveTiles;
            // Remove squad from selected squads if being deselected
            if (activeTiles.ContainsKey(tileModel.TileId) && !tileModel.IsFlipped)
            {
                activeTiles.Remove(tileModel.TileId);
                _activeTilesUpdate.OnNext(activeTiles);
            }
            // Add squad to selected squads if recently selected
            else if (tileModel.IsFlipped && !activeTiles.ContainsKey(tileModel.TileId))
            {
                activeTiles.Add(tileModel.TileId, tileModel);
                _activeTilesUpdate.OnNext(activeTiles);
            }

            _allTilesUpdate.OnNext(_gameService.Game.AllTiles);
        }
        #endregion

        #region Helper Methods
        private void Dispose()
        {
            if (_tilesUpdateSubscription != null) _tilesUpdateSubscription.Dispose();
        }

        private void InitializeAllTiles()
        {
            Dictionary<string, TileModel> allTiles = new Dictionary<string, TileModel>();

            var tileDictionaries = new Dictionary<string, int[]>[]
            {
                _resourceMissions,
                _doomsdayMissions,
                _agentHeadquarters
            };

            foreach(Dictionary<string, int[]> tileDictionary in tileDictionaries)
            {
                foreach (KeyValuePair<string, int[]> tile in tileDictionary)
                {
                    TileModel tileModel = new TileModel
                    {
                        TileId = tile.Value[0].ToString(),
                        TileName = tile.Key,
                        Stats = new Dictionary<string, int>
                        {
                            { "Combat", tile.Value[1] },
                            { "Stealth", tile.Value[2] },
                            { "Cunning", tile.Value[3] },
                            { "Diplomacy", tile.Value[4] }
                        }
                    };
                    allTiles.Add(tileModel.TileId, tileModel);
                }
            }
            _gameService.Game.AllTiles = allTiles;
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
            _tilesUpdateSubscription = _gameDataService.DataDictReceived.Subscribe(tileModels =>
            {
                throw new NotImplementedException();
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
