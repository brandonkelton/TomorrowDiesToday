using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public class SquadService : ISquadService
    {
        #region Properties
        // Observables
        public IObservable<string> ErrorMessage => _errorMessage;
        public IObservable<List<SquadModel>> SelectedSquadsUpdate => _selectedSquadsUpdate;
        public IObservable<SquadModel> SquadUpdate => _squadUpdate;
        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        private readonly ReplaySubject<List<SquadModel>> _selectedSquadsUpdate = new ReplaySubject<List<SquadModel>>(1);
        private readonly ReplaySubject<SquadModel> _squadUpdate = new ReplaySubject<SquadModel>(1);

        // Requred Service(s)
        private IGameService _gameService;

        // Constants
        private const int MAX_SQUAD_SIZE = 6;
        private const int NUMBER_OF_FACED_HENCHMAN = 9;
        private const int DATA_STRIP_LENGTH = 13;

        // Miscellaneous
        public string[] NamedHenchmenNames => _namedHenchmenNames;
        public Dictionary<string, int[]> NamedHenchmenStats => _namedHenchmenStats;
        private string[] _namedHenchmenNames = new string[]
        {
            "General Goodman",
            "Archibald Kluge",
            "Axle Robbins",
            "Azura Badeau",
            "Boris 'Myasneek'",
            "Cassandra O'Shea",
            "Emerson Barlow",
            "Jin Feng",
            "The Node",
            "Ugo Dottore"
        };
        private Dictionary<string, int[]> _namedHenchmenStats = new Dictionary<string, int[]>
        {
            // Name, { ID, Combat, Stealth, Cunning, Diplomacy }
            { "General Goodman",    new int[] {0, 2, 2, 2, 2} },
            { "Archibald Kluge",    new int[] {1, 0, 1, 3, 1} },
            { "Axle Robbins",       new int[] {2, 1, 0, 2, 2} },
            { "Azura Badeau",       new int[] {3, 2, 2, 1, 0} },
            { "Boris 'Myasneek'", new int[] {4, 3, 1, 1, 0} },
            { "Cassandra O'Shea",   new int[] {5, 0, 0, 2, 3} },
            { "Emerson Barlow",     new int[] {6, 1, 3, 1, 0} },
            { "Jin Feng",           new int[] {7, 0, 3, 1, 1} },
            { "The Node",           new int[] {8, 0, 2, 2, 1} },
            { "Ugo Dottore",        new int[] {9, 1, 0, 3, 1} },
        };
        #endregion

        #region Public Methods
        public void CalculateSquadStats(SquadModel squadModel)
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();

            stats.Add("Combat", CalculateCombat(squadModel.Data));
            stats.Add("Stealth", CalculateStealth(squadModel.Data));
            stats.Add("Cunning", CalculateCunning(squadModel.Data));
            stats.Add("Diplomacy", CalculateDiplomacy(squadModel.Data));

            squadModel.Stats = stats;
            UpdateSquad(squadModel);
        }

        public void ToggleSquad(SquadModel squadModel)
        {
            squadModel.IsSelected = !squadModel.IsSelected;
            var players = _gameService.Game.Players;
            var selectedSquads = players.SelectMany(player => player.Squads.Where(squad => squad.IsSelected)).ToList();
            _selectedSquadsUpdate.OnNext(selectedSquads);
            _squadUpdate.OnNext(squadModel);
        }
        #endregion

        #region Helper Methods
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

        private bool ValidateSquad(SquadModel squadModel)
        {
            var squadData = squadModel.Data;
            int unitTotal = squadData["Thief"] + squadData["Hacker"] + squadData["Soldier"]
                + squadData["Assassin"] + squadData["Fixer"] + squadData["Scientist"];
            int ugoTotal = squadData["Ugo Combat"] + squadData["Ugo Stealth"] + squadData["Ugo Cunning"] + squadData["Ugo Diplomacy"];

            string validationError = "";

            if (unitTotal > MAX_SQUAD_SIZE || unitTotal < 0)
            {
                validationError += "Invalid squad size\n";
            }
            if (squadData["Named Henchman"] > NUMBER_OF_FACED_HENCHMAN || squadData["Named Henchman"] < 0)
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
        #endregion
    }
}
