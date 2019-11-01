using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Game
{
    public class GameService : IGameService
    {
        public GameModel ThisGame
        {
            get { return _game; }
            set { _game = value; }
        }

        public IObservable<string> ErrorMessage => _errorMessage;
        public IObservable<Dictionary<string, PlayerModel>> OtherPlayers => _otherPlayers;
        public IObservable<PlayerModel> ThisPlayer => _thisPlayer;
        public IObservable<Dictionary<string, TileModel>> Tiles => _tiles;

        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        private readonly ReplaySubject<Dictionary<string, PlayerModel>> _otherPlayers
            = new ReplaySubject<Dictionary<string, PlayerModel>>(1); // { PlayerId => PlayerModel }
        private readonly ReplaySubject<PlayerModel> _thisPlayer
            = new ReplaySubject<PlayerModel>(1); 
        private readonly ReplaySubject<Dictionary<string, TileModel>> _tiles
            = new ReplaySubject<Dictionary<string, TileModel>>(1); // { TileId => TileModel }

        private const int MAX_SQUAD_SIZE = 6;
        private const int NUMBER_OF_FACED_HENCHMAN = 9;
        private const int DATA_STRIP_LENGTH = 13;

        private Dictionary<string, SquadModel> _selectedSquads = new Dictionary<string, SquadModel>();
        private GameModel _game;
        private IDataService<GameModel, GameRequest> _gameDataService;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;
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
        private static Random _random = new Random();

        // PlayerDataService and GameDataService observables to subscribe to
        private IDisposable _playerUpdateSubscription = null;
        private IDisposable _playerUpdateDictSubscription = null;
        private IDisposable _tilesUpdateSubscription = null;

        public async Task ChoosePlayer(string playerId)
        {
            if (! await _playerDataService.Exists(playerId))
            {
                await _playerDataService.Create(playerId);

                PlayerModel playerModel = new PlayerModel
                {
                    PlayerId = playerId,
                    Squads = new Dictionary<string, SquadModel>()
                };

                UpdateThisPlayer(playerModel);
            }
            else
            {
                _errorMessage.OnNext("Choose again!");
            }
        }

        public async Task CreateGame()
        {
            bool gameExists = true;
            string gameId = "";
            while (gameExists)
            {
                gameId = GenerateGameId();
                gameExists = await _gameDataService.Exists(gameId);
            }
            _game.GameId = gameId;
        }

        public GameService(IDataService<GameModel, GameRequest> gameData, IDataService<PlayerModel, PlayerRequest> playerData)
        {
            _gameDataService = gameData;
            _playerDataService = playerData;

            SubscribeToUpdates();
        }

        public void FlipTile(TileModel tileModel)
        {
            _game.Tiles[tileModel.TileId].IsFlipped = !_game.Tiles[tileModel.TileId].IsFlipped;
            UpdateTiles(_game.Tiles);
        }

        public string GenerateGameId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string gameId = new string(Enumerable.Repeat(chars, chars.Length)
              .Select(s => s[_random.Next(s.Length)]).Take(6).ToArray());
            return gameId;
        }

        public async Task JoinGame(string gameId)
        {
            if (await _gameDataService.Exists(gameId))
            {
                _game.GameId = gameId;
            }
            else
            {
                _errorMessage.OnNext("Game doesn't exist!");
            }
        }

        public async Task RequestPlayerUpdate(PlayerModel playerModel)
        {
            PlayerRequest playerRequest = new PlayerRequest { PlayerId = playerModel.PlayerId };
            await _playerDataService.RequestUpdate(playerRequest);
        }

        public async Task RequestPlayersUpdate()
        {
            PlayerRequest playerRequest = new PlayerRequest();
            await _playerDataService.RequestUpdate(playerRequest);
        }

        public async Task RequestTilesUpdate()
        {
            PlayerRequest playerRequest = new PlayerRequest();
            await _playerDataService.RequestUpdate(playerRequest);
        }

        public async Task SendThisPlayer()
        {
            await _playerDataService.Update(_game.ThisPlayer);
        }

        public async Task SendTiles()
        {
            await _gameDataService.Update(_game);
        }

        public void ToggleSquad(SquadModel squadModel)
        {
            squadModel.IsSelected = !squadModel.IsSelected;

            // Remove squad from selected squads if being deselected
            if (!squadModel.IsSelected)
            {
                _selectedSquads.Remove(squadModel.SquadId);
            }
            else // Add squad to selected squads if being selected
            {
                _selectedSquads.Add(squadModel.SquadId, squadModel);
            }
            UpdateSquad(squadModel);
        }

        public void UpdateSquad(SquadModel squadModel)
        {
            if (squadModel.PlayerId == _game.ThisPlayer.PlayerId)
            {
                _game.ThisPlayer.Squads[squadModel.SquadId] = squadModel;
                _thisPlayer.OnNext(_game.ThisPlayer);
            }
            else
            {
                _game.OtherPlayers[squadModel.PlayerId].Squads[squadModel.SquadId] = squadModel;
                _otherPlayers.OnNext(_game.OtherPlayers);
            }
        }

        private Dictionary<string, int> AddSquadStats(params Dictionary<string, int>[] squads)
        {
            int combatTotal = 0;
            int stealthTotal = 0;
            int cunningTotal = 0;
            int diplomacyTotal = 0;
            Dictionary<string, int> squadTotals = new Dictionary<string, int>();

            foreach (Dictionary<string, int> squad in squads)
            {
                combatTotal += squad["Combat"];
                stealthTotal += squad["Stealth"];
                cunningTotal += squad["Cunning"];
                diplomacyTotal += squad["Diplomacy"];
            }

            squadTotals.Add("Combat", combatTotal);
            squadTotals.Add("Stealth", stealthTotal);
            squadTotals.Add("Cunning", cunningTotal);
            squadTotals.Add("Diplomacy", diplomacyTotal);

            return squadTotals;
        }

        private int CalculateCombat(Dictionary<string, int> squadData)
        {
            int total = 0;
            int facedHenchmen = squadData["Faced Henchmen"];
            int soldiers = squadData["Soldier"];
            int assassins = squadData["Assassin"];

            total += (soldiers * 2);
            total += assassins;

            switch (facedHenchmen)
            {
                //Axle Robbins
                case 2:
                    total += 1;
                    break;

                //Azura Badeau
                case 3:
                    total += 2;
                    break;

                //Boris "Myasneek"
                case 4:
                    total += 3;
                    break;

                //Emerson Barlow
                case 6:
                    total += 1;
                    break;

                //Ugo Dottore
                case 9:
                    total += 1;
                    break;

                default:
                    break;
            }

            if (facedHenchmen == 9)
            {
                total += squadData["Ugo Combat"];
            }

            if (squadData["Explosive Rounds"] == 1)
            {
                total += 2;
            }

            return total;
        }

        private Dictionary<string, int> CalculateSquadStats(Dictionary<string, int> squadData)
        {
            Dictionary<string, int> squadStats = new Dictionary<string, int>();

            squadStats.Add("Combat", CalculateCombat(squadData));
            squadStats.Add("Stealth", CalculateStealth(squadData));
            squadStats.Add("Cunning", CalculateCunning(squadData));
            squadStats.Add("Diplomacy", CalculateDiplomacy(squadData));

            return squadStats;
        }

        private int CalculateStealth(Dictionary<string, int> squadData)
        {
            int total = 0;
            int facedHenchmen = squadData["Faced Henchmen"];
            int soldiers = squadData["Soldier"];
            int assassins = squadData["Assassin"];
            int thieves = squadData["Thief"];
            int hackers = squadData["Hacker"];

            total += soldiers;
            total += (assassins * 2);
            total += (thieves * 2);
            total += hackers;

            switch (facedHenchmen)
            {
                //Archibald Kluge
                case 1:
                    total += 1;
                    break;

                //Azura Badeau
                case 3:
                    total += 2;
                    break;

                //Boris "Myasneek"
                case 4:
                    total += 1;
                    break;

                //Emmerson Barlow
                case 6:
                    total += 3;
                    break;

                //Jin Feng
                case 7:
                    total += 3;
                    break;

                //The Node
                case 8:
                    total += 2;
                    break;

                default:
                    break;
            }

            if (facedHenchmen == 9)
            {
                total += squadData["Ugo Stealth"];
            }

            return total;
        }

        private int CalculateCunning(Dictionary<string, int> squadData)
        {
            int total = 0;
            int facedHenchmen = squadData["Faced Henchmen"];
            int thieves = squadData["Thief"];
            int scientists = squadData["Scientist"];
            int fixers = squadData["Fixer"];
            int hackers = squadData["Hacker"];

            total += thieves;
            total += (scientists * 2);
            total += fixers;
            total += (hackers * 2);

            switch (facedHenchmen)
            {
                //Archibald Kluge
                case 1:
                    total += 3;
                    break;

                //"Axle" Robbins
                case 2:
                    total += 2;
                    break;

                //Azura Badeau
                case 3:
                    total += 1;
                    break;

                //Boris "Myasneek"
                case 4:
                    total += 1;
                    break;

                //Cassandra O'Shea
                case 5:
                    total += 2;
                    break;

                //Emmerson Barlow
                case 6:
                    total += 1;
                    break;

                //Jin Feng
                case 7:
                    total += 1;
                    break;

                //The Node
                case 8:
                    total += 2;
                    break;

                //Ugo Dottore
                case 9:
                    total += 3;
                    break;

                default:
                    break;
            }

            if (facedHenchmen == 9)
            {
                total += squadData["Ugo Cunning"];
            }

            return total;
        }
        private int CalculateDiplomacy(Dictionary<string, int> squadData)
        {
            int total = 0;
            int facedHenchmen = squadData["Faced Henchmen"];
            int scientists = squadData["Scientist"];
            int fixers = squadData["Fixer"];

            total += scientists;
            total += (fixers * 2);

            switch (facedHenchmen)
            {
                //Archibald Kluge
                case 1:
                    total += 1;
                    break;

                //"Axle" Robbins
                case 2:
                    total += 2;
                    break;

                //Cassandra O'Shea
                case 5:
                    total += 3;
                    break;

                //Jin Feng
                case 7:
                    total += 1;
                    break;

                //The Node
                case 8:
                    total += 1;
                    break;

                case 9:
                    total += 1;
                    break;

                default:
                    break;
            }

            if (squadData["Hypnotic Spray"] == 1)
            {
                total += 2;
            }

            if (facedHenchmen == 9)
            {
                total += squadData["Ugo Diplomacy"];
            }

            return total;
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
            _playerUpdateSubscription = _playerDataService.DataReceived.Subscribe(playerModel =>
            {
                UpdateOtherPlayer(playerModel);
            });
            _playerUpdateSubscription = _playerDataService.DataDictReceived.Subscribe(playerModels =>
            {
                UpdateOtherPlayers(playerModels);
            });
            _tilesUpdateSubscription = _gameDataService.DataReceived.Subscribe(gameModel =>
            {
                UpdateTiles(gameModel.Tiles);
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

        private void UpdateOtherPlayer(PlayerModel playerModel)
        {
            _game.OtherPlayers[playerModel.PlayerId] = playerModel;
            _otherPlayers.OnNext(_game.OtherPlayers);
        }

        private void UpdateOtherPlayers(Dictionary<string, PlayerModel> playerModels)
        {
            _game.OtherPlayers = playerModels;
            _otherPlayers.OnNext(_game.OtherPlayers);
        }

        private void UpdateThisPlayer(PlayerModel playerModel)
        {
            _game.ThisPlayer = playerModel;
            _thisPlayer.OnNext(_game.ThisPlayer);
        }

        private void UpdateTiles(Dictionary<string, TileModel> tileModels)
        {
            _game.Tiles = tileModels;
            _tiles.OnNext(_game.Tiles);
        }

        private bool ValidateSquad(Dictionary<string, int> squadData)
        {
            int unitTotal = squadData["Thief"] + squadData["Hacker"] + squadData["Soldier"]
                + squadData["Assassin"] + squadData["Fixer"] + squadData["Scientist"];
            int ugoTotal = squadData["Ugo Combat"] + squadData["Ugo Stealth"] + squadData["Ugo Cunning"] + squadData["Ugo Diplomacy"];

            string validationError = "";

            if (unitTotal > MAX_SQUAD_SIZE || unitTotal < 0)
            {
                validationError += "Invalid squad size\n";
            }
            if (squadData["Faced Henchman"] > NUMBER_OF_FACED_HENCHMAN || squadData["Faced Henchman"] < 0)
            {
                validationError += "Invalid number of named henchmen\n";
            }
            if (squadData["Hypnotic Spray"] > 1 || squadData["Hypnotic Spray"] < 0)
            {
                throw new NotImplementedException();
            }
            if (squadData["Explosive Rounds"] > 1 || squadData["Explosive Rounds"] < 0)
            {
                validationError += "\n";
            }
            if (ugoTotal > MAX_SQUAD_SIZE || ugoTotal < 0)
            {
                validationError += "\n";
            }
            if (validationError != "")
            {
                _errorMessage.OnNext(validationError);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
