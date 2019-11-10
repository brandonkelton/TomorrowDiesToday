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
        
        public IObservable<Dictionary<string, TileModel>> ActiveTilesUpdate => _activeTilesUpdate;
        public IObservable<Dictionary<string, TileModel>> AllTilesUpdate => _allTilesUpdate;
        public IObservable<string> ErrorMessage => _errorMessage;

        private readonly ReplaySubject<Dictionary<string, TileModel>> _activeTilesUpdate = new ReplaySubject<Dictionary<string, TileModel>>(1);
        private readonly ReplaySubject<Dictionary<string, TileModel>> _allTilesUpdate = new ReplaySubject<Dictionary<string, TileModel>>(1);
        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);

        private Dictionary<string, string> _flipMissions = new Dictionary<string, string> //resourceLookupDictionary <TileName, TileStats>
        {
            { "Blood Diamond Harvest", "3,2,2,0" },
            { "Skin Trade", "1,1,2,3" },
            { "Social Engineering Scams", "0,1,3,1" },
            { "Counterfeiting Operation", "0,3,2,0" },
            { "Ponzi Schemes", "0,1,3,2" },
            { "Political Corruption", "0,1,2,2" },
            { "Hacker Cell", "0,3,4,0" },
            { "Art Thievery", "0,3,3,1" },
            { "Exotic Car GTA", "0,3,3,0" },
            { "Smuggling Ring", "0,2,2,2" },
            { "Arms Dealing", "1,3,1,0" },
            { "Maritime Piracy", "3,2,2,0" },
            { "Rig Sports Events", "0,1,2,2" },
            { "Gambling Dens", "1,1,3,0" },
            { "Narcotics Distribution", "1,3,2,1" },
            { "CBRNE Dealing", "1,0,4,2" },
            { "Ivory Poaching", "3,2,2,0" },
            { "Murder Inc", "2,4,1,0"}
        };
        private Dictionary<string, string> _missions = new Dictionary<string, string>
        { //<TileName, TileStats>
            //Resource Missions
            { "Blood Diamond Harvest", "3,2,1,0" },
            { "Skin Trade", "1,1,2,2" },
            { "Social Engineering Scams", "0,1,3,2" },
            { "Counterfeiting Operation", "0,3,2,1" },
            { "Ponzi Schemes", "0,1,2,3" },
            { "Political Corruption", "0,2,2,2" },
            { "Hacker Cell", "0,3,3,0" },
            { "Art Thievery", "1,2,2,1" },
            { "Exotic Car GTA", "0,2,4,0" },
            { "Smuggling Ring", "0,4,1,1" },
            { "Arms Dealing", "3,1,1,1" },
            { "Maritime Piracy", "3,1,2,0" },
            { "Rig Sports Events", "0,2,3,1" },
            { "Gambling Dens", "1,2,3,0" },
            { "Narcotics Distribution", "1,2,1,2" },
            { "CBRNE Dealing", "1,0,4,1" },
            { "Ivory Poaching", "3,0,2,1" },
            { "Murder Inc", "1,3,1,1" },
            //Doomsday Missions
            { "Crash Wall Street", "0,6,8,1" },
            { "Destroy IXP's", "2,6,6,1" },
            { "Bring Down Satellites", "2,2,7,4" },
            { "Kidnap Military & Political Leaders", "3,6,3,3" },
            { "Deploy Neurotoxin", "4,3,6,2" },
            { "Acquire Russian Nuclear Stockpile", "6,5,4,0" },
            { "Burgle Fort Knox", "1,8,4,2" },
            { "Supplant Major African Warlords", "8,4,1,2" },
            { "Take Over South American Cartels", "4,8,1,2" },
            { "Hijack Major World Media Outlets", "0,2,7,6" },
            { "Infiltrate Asian Intelligence Agencies", "3,4,4,4" },
            { "Corrupt the U.N.", "0,2,5,8" },
            //Agent Headquarters
            { "CIA Building", "2,2,2,2"},
            { "Interpol HQ", "1,2,2,1"}
        };

        private IDisposable _tilesUpdateSubscription = null;

        private IDataService<GameModel, GameRequest> _gameDataService;
        private IGameService _gameService;

        public TileService(IDataService<GameModel, GameRequest> gameDataService, IGameService gameService)
        {
            _gameService = gameService;
            _gameDataService = gameDataService;
            InitializeAllTiles();
            SubscribeToUpdates();
        }

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

            UpdateAllTiles();
        }

        #region Helper Methods
        private void Dispose()
        {
            if (_tilesUpdateSubscription != null) _tilesUpdateSubscription.Dispose();
        }

        private void InitializeAllTiles()
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, int> HandleTileAlerts(int alerts, Dictionary<string, int> tileDictionary)
        {

            if (alerts > 0)
            {
                tileDictionary["Combat"] *= alerts;
                tileDictionary["Stealth"] *= alerts;
                tileDictionary["Cunning"] *= alerts;
                tileDictionary["Diplomacy"] *= alerts;
            }

            return tileDictionary;
        }

        private Dictionary<string, int> ParseToDictionary(string dataStrip)
        {
            string[] keyArray = new string[] { "Combat", "Stealth", "Cunning", "Diplomacy" };
            string[] stringArray = dataStrip.Split(',');
            int[] statusValueArray = Array.ConvertAll(stringArray, int.Parse);
            Dictionary<string, int> tileStatus = new Dictionary<string, int>();

            if (keyArray.Length == statusValueArray.Length)
            {
                for (int i = 0; i < keyArray.Length; i++)
                {
                    tileStatus.Add(keyArray[i], statusValueArray[i]);
                }
            }

            return tileStatus;
        }

        private void SubscribeToUpdates()
        {
            _tilesUpdateSubscription = _gameDataService.DataDictReceived.Subscribe(tileModels =>
            {
                throw new NotImplementedException();
            });
        }

        private bool SuccessCheck(Dictionary<string, int> tileStats, Dictionary<string, int> squadStats)
        {
            int combatResult = squadStats["Combat"] - tileStats["Combat"];
            int stealthResult = squadStats["Stealth"] - tileStats["Stealth"];
            int cunningResult = squadStats["Cunning"] - tileStats["Cunning"];
            int diplomacyResult = squadStats["Diplomacy"] - tileStats["Diplomacy"];

            if (combatResult >= 0 && stealthResult >= 0 && cunningResult >= 0 && diplomacyResult >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Dictionary<string, int> TileLookup(string tileName, Boolean flipped, int alerts) //returns Dictionary of matching name
        {
            Dictionary<string, int> tileDictionary;
            string dataStrip;

            if (flipped == true)
            {
                dataStrip = _flipMissions[tileName];
            }
            else if (flipped == false)
            {
                dataStrip = _missions[tileName];
            }
            else
            {
                //throw some exception
                System.ArgumentException argEx = new System.ArgumentException("Index is out of range", "index");
                throw argEx;
            }

            tileDictionary = ParseToDictionary(dataStrip);
            if (alerts > 0)
            {
                HandleTileAlerts(alerts, tileDictionary);
            }

            return tileDictionary;
        }

        private void UpdateActiveTiles()
        {
            _activeTilesUpdate.OnNext(_gameService.Game.ActiveTiles);
        }

        private void UpdateAllTiles()
        {
            _allTilesUpdate.OnNext(_gameService.Game.AllTiles);
        }
        #endregion
    }
}
