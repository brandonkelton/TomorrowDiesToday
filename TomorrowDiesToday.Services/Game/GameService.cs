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
        public GameModel Game { get; set; }

        private Dictionary<int, string> _playerLookupTable = new Dictionary<int, string>
        {
            { 0,  "General Goodman" },
            { 1,  "Archibald Kluge" },
            { 2,  "Axle Robbins" },
            { 3,  "Azura Badeau" },
            { 4,  "Boris 'Myasneek'" },
            { 5,  "Cassandra O'Shea" },
            { 6,  "Emerson Barlow" },
            { 7,  "Jin Feng" },
            { 8,  "The Node" },
            { 9,  "Ugo Dottore" },
        };
        public Dictionary<int, string> PlayerLookup
        {
            get { return _playerLookupTable; }
        }

        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        public IObservable<string> ErrorMessage => _errorMessage;

        private readonly ReplaySubject<Dictionary<string, PlayerModel>> _otherPlayers
            = new ReplaySubject<Dictionary<string, PlayerModel>>(1); // { PlayerId => PlayerModel }
        public IObservable<Dictionary<string, PlayerModel>> OtherPlayers => _otherPlayers;

        private readonly ReplaySubject<GameModel> _thisGame
            = new ReplaySubject<GameModel>(1);
        public IObservable<GameModel> ThisGame => _thisGame;

        private readonly ReplaySubject<PlayerModel> _thisPlayer
            = new ReplaySubject<PlayerModel>(1);
        public IObservable<PlayerModel> ThisPlayer => _thisPlayer;

        private readonly ReplaySubject<Dictionary<string, TileModel>> _tiles
            = new ReplaySubject<Dictionary<string, TileModel>>(1); // { TileId => TileModel }
        public IObservable<Dictionary<string, TileModel>> Tiles => _tiles;

        private const int MAX_SQUAD_SIZE = 6;
        private const int NUMBER_OF_FACED_HENCHMAN = 9;
        private const int DATA_STRIP_LENGTH = 13;

        private IDataService<GameModel, GameRequest> _gameDataService;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;

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
        private Dictionary<string, Dictionary<string, int>> _playerStats = new Dictionary<string, Dictionary<string, int>>
        {
            {
                "General Goodman", new Dictionary<string, int>
                 {
                    {"Id", 0 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            },
            {
                "Archibald Kluge", new Dictionary<string, int>
                 {
                    {"Id", 1 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            },
            {
                "Axle Robbins", new Dictionary<string, int>
                 {
                    {"Id", 2 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            },
            {
                "Azura Badeau", new Dictionary<string, int>
                 {
                    {"Id", 3 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            },
            {
                "Boris Myasneek", new Dictionary<string, int>
                 {
                    {"Id", 4 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            },
            {
                "Cassandra O'Shea", new Dictionary<string, int>
                 {
                    {"Id", 5 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            },
            {
                "Emerson Barlow", new Dictionary<string, int>
                 {
                    {"Id", 6 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            },
            {
                "Jin Feng", new Dictionary<string, int>
                 {
                    {"Id", 7 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            },
            {
                "The Node", new Dictionary<string, int>
                 {
                    {"Id", 8 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            },
            {
                "Ugo Dottore", new Dictionary<string, int>
                 {
                    {"Id", 9 },
                    {"Combat", 0 },
                    {"Stealth", 0 },
                    {"Cunning", 0 },
                    {"Diplomacy", 0 }
                 }
            }
        };
        private static Random _random = new Random();
        private Dictionary<string, SquadModel> _selectedSquads = new Dictionary<string, SquadModel>();

        // PlayerDataService and GameDataService observables to subscribe to
        private IDisposable _playerUpdateSubscription = null;
        private IDisposable _playerUpdateDictSubscription = null;
        private IDisposable _tilesUpdateSubscription = null;

        public GameService(IDataService<GameModel, GameRequest> gameData, IDataService<PlayerModel, PlayerRequest> playerData)
        {
            _gameDataService = gameData;
            _playerDataService = playerData;

            SubscribeToUpdates();
        }

        public async Task<bool> ChoosePlayer(string playerName)
        {
            string playerId = _playerStats[playerName]["Id"].ToString();
            PlayerRequest request = new PlayerRequest
            {
                GameId = Game.GameId,
                PlayerId = playerId
            };
            if (! await _playerDataService.Exists(request))
            {
                await _playerDataService.Create(request);
                PlayerModel playerModel = GeneratePlayer(playerId);
                UpdateThisPlayer(playerModel);
                return true;
            }
            else
            {
                _errorMessage.OnNext("Choose again!");
                return false;
            }
        }

        public async Task CreateGame()
        {
            bool gameExists = true;
            string gameId = "";
            while (gameExists)
            {
                gameId = GenerateGameId();
                GameRequest request = new GameRequest { GameId = gameId };
                gameExists = await _gameDataService.Exists(request);
            }
            Game = new GameModel { GameId = gameId };
            _thisGame.OnNext(Game);
        }

        public async Task<bool> JoinGame(string gameId)
        {
            GameRequest request = new GameRequest { GameId = gameId };
            if (await _gameDataService.Exists(request))
            {
                //await _gameDataService.Exists(request);
                Game = new GameModel { GameId = gameId };
                _thisGame.OnNext(Game);
                return true;
            }
            else
            {
                _errorMessage.OnNext("Game doesn't exist!");
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
            await _playerDataService.Update(Game.ThisPlayer);
        }

        public async Task SendTiles()
        {
            await _gameDataService.Update(Game);
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
            if (squadModel.PlayerId == Game.ThisPlayer.PlayerId)
            {
                UpdateSquad(squadModel);
            }
            else
            {
                Game.OtherPlayers[squadModel.PlayerId].Squads[squadModel.SquadId] = squadModel;
                UpdateOtherPlayer(Game.OtherPlayers[squadModel.PlayerId]);
            }
        }

        public void ToggleTile(TileModel tileModel)
        {
            Game.Tiles[tileModel.TileId].IsFlipped = !Game.Tiles[tileModel.TileId].IsFlipped;
            UpdateTiles(Game.Tiles);
        }

        public void UpdateSquad(SquadModel squadModel)
        {
            if (ValidateSquad(squadModel.Data))
            {
                Game.ThisPlayer.Squads[squadModel.SquadId] = squadModel;
                _thisPlayer.OnNext(Game.ThisPlayer);
            }
        }

        public void UpdateTile(TileModel tileModel)
        {
            Game.Tiles[tileModel.TileId] = tileModel;
            _tiles.OnNext(Game.Tiles);
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
            int namedHenchman = squadData["Named Henchman"];
            int soldiers = squadData["Soldier"];
            int assassins = squadData["Assassin"];

            total += (soldiers * 2);
            total += assassins;

            switch (namedHenchman)
            {
                // Axle Robbins
                case 2:
                    total += 1;
                    break;

                // Azura Badeau
                case 3:
                    total += 2;
                    break;

                // Boris "Myasneek"
                case 4:
                    total += 3;
                    break;

                // Emerson Barlow
                case 6:
                    total += 1;
                    break;

                // Ugo Dottore
                case 9:
                    total += 1;
                    break;

                default:
                    break;
            }

            if (namedHenchman == 9)
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
            int totalStealth = 0;
            int namedHenchman = squadData["Named Henchman"];
            int soldiers = squadData["Soldier"];
            int assassins = squadData["Assassin"];
            int thieves = squadData["Thief"];
            int hackers = squadData["Hacker"];

            totalStealth += soldiers;
            totalStealth += (assassins * 2);
            totalStealth += (thieves * 2);
            totalStealth += hackers;

            switch (namedHenchman)
            {
                // Archibald Kluge
                case 1:
                    totalStealth += 1;
                    break;

                // Azura Badeau
                case 3:
                    totalStealth += 2;
                    break;

                // Boris "Myasneek"
                case 4:
                    totalStealth += 1;
                    break;

                // Emmerson Barlow
                case 6:
                    totalStealth += 3;
                    break;

                // Jin Feng
                case 7:
                    totalStealth += 3;
                    break;

                // The Node
                case 8:
                    totalStealth += 2;
                    break;

                default:
                    break;
            }

            if (namedHenchman == 9)
            {
                totalStealth += squadData["Ugo Stealth"];
            }

            return totalStealth;
        }

        private int CalculateCunning(Dictionary<string, int> squadData)
        {
            int total = 0;
            int namedHenchman = squadData["Named Henchman"];
            int thieves = squadData["Thief"];
            int scientists = squadData["Scientist"];
            int fixers = squadData["Fixer"];
            int hackers = squadData["Hacker"];

            total += thieves;
            total += (scientists * 2);
            total += fixers;
            total += (hackers * 2);

            switch (namedHenchman)
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

            if (namedHenchman == 9)
            {
                total += squadData["Ugo Cunning"];
            }

            return total;
        }
        private int CalculateDiplomacy(Dictionary<string, int> squadData)
        {
            int total = 0;
            int namedHenchman = squadData["Named Henchman"];
            int scientists = squadData["Scientist"];
            int fixers = squadData["Fixer"];

            total += scientists;
            total += (fixers * 2);

            switch (namedHenchman)
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

            if (namedHenchman == 9)
            {
                total += squadData["Ugo Diplomacy"];
            }

            return total;
        }

        private void Dispose()
        {
            if (_playerUpdateSubscription != null) _playerUpdateSubscription.Dispose();
            if (_playerUpdateDictSubscription != null) _playerUpdateDictSubscription.Dispose();
            if (_tilesUpdateSubscription != null) _tilesUpdateSubscription.Dispose();
        }

        private string GenerateGameId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string gameId = new string(Enumerable.Repeat(chars, chars.Length)
              .Select(s => s[_random.Next(s.Length)]).Take(6).ToArray());

            return gameId;
        }

        private PlayerModel GeneratePlayer(string playerId)
        {
            PlayerModel playerModel = new PlayerModel
            {
                GameId = Game.GameId,
                PlayerId = playerId,
                PlayerName = PlayerLookup[int.Parse(playerId)],
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
            string squadId = string.Format("{0}-1", playerId);
            playerModel.Squads[squadId].Data["Named Henchman"] = int.Parse(playerId);

            // Calculate stats for squad 1
            playerModel.Squads[squadId].Stats = CalculateSquadStats(playerModel.Squads[squadId].Data);

            return playerModel;
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
                playerModel.PlayerName = PlayerLookup[int.Parse(playerModel.PlayerId)];
                UpdateOtherPlayer(playerModel);
            });
            _playerUpdateSubscription = _playerDataService.DataDictReceived.Subscribe(playerModels =>
            {
                foreach(KeyValuePair<string, PlayerModel> player in playerModels)
                {
                    player.Value.PlayerName = PlayerLookup[int.Parse(player.Value.PlayerId)];
                }
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
            Game.OtherPlayers[playerModel.PlayerId] = playerModel;
            _otherPlayers.OnNext(Game.OtherPlayers);
        }

        private void UpdateOtherPlayers(Dictionary<string, PlayerModel> playerModels)
        {
            Game.OtherPlayers = playerModels;
            _otherPlayers.OnNext(Game.OtherPlayers);
        }

        private void UpdateThisPlayer(PlayerModel playerModel)
        {
            Game.ThisPlayer = playerModel;
            _thisPlayer.OnNext(Game.ThisPlayer);
        }

        private void UpdateTiles(Dictionary<string, TileModel> tileModels)
        {
            Game.Tiles = tileModels;
            _tiles.OnNext(Game.Tiles);
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
