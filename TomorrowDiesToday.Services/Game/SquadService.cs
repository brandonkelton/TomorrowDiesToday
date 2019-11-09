using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public class SquadService : ISquadService
    {
        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        public IObservable<string> ErrorMessage => _errorMessage;

        private readonly ReplaySubject<Dictionary<string, SquadModel>> _selectedSquadsUpdate
            = new ReplaySubject<Dictionary<string, SquadModel>>(1);
        public IObservable<Dictionary<string, SquadModel>> SelectedSquadsUpdate => _selectedSquadsUpdate;

        private readonly ReplaySubject<SquadModel> _squadUpdate
            = new ReplaySubject<SquadModel>(1);
        public IObservable<SquadModel> SquadUpdate => _squadUpdate;

        private const int MAX_SQUAD_SIZE = 6;
        private const int NUMBER_OF_FACED_HENCHMAN = 9;
        private const int DATA_STRIP_LENGTH = 13;

        private IGameService _gameService;

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

            var selectedSquads = _gameService.Game.SelectedSquads;
            // Remove squad from selected squads if being deselected
            if (selectedSquads.ContainsKey(squadModel.SquadId) && !squadModel.IsSelected)
            {
                selectedSquads.Remove(squadModel.SquadId);
                _selectedSquadsUpdate.OnNext(selectedSquads);
            }
            // Add squad to selected squads if recently selected
            else if (squadModel.IsSelected && !selectedSquads.ContainsKey(squadModel.SquadId))
            {
                selectedSquads.Add(squadModel.SquadId, squadModel);
                _selectedSquadsUpdate.OnNext(selectedSquads);
            }

            UpdateSquad(squadModel);
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

        private void UpdateSquad(SquadModel squadModel)
        {
            if (ValidateSquad(squadModel))
            {
                _squadUpdate.OnNext(squadModel);
            }
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
    }
}
