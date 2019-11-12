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

        #endregion

        #region Public Methods

        public void CalculateSquadStats(SquadModel squadModel)
        {
            SquadStats stats = new SquadStats();

            stats.Combat.SetValue(squadModel.Armaments.Sum(armament => armament.Count * armament.Stats.Combat.Value));
            stats.Stealth.SetValue(squadModel.Armaments.Sum(armament => armament.Count * armament.Stats.Stealth.Value));
            stats.Cunning.SetValue(squadModel.Armaments.Sum(armament => armament.Count * armament.Stats.Cunning.Value));
            stats.Diplomacy.SetValue(squadModel.Armaments.Sum(armament => armament.Count * armament.Stats.Diplomacy.Value));

            if (squadModel.IsSelected)
            {
                var selectedSquads = _gameService.Game.Players.SelectMany(player => player.Squads.Where(squad => squad.IsSelected)).ToList();
                _selectedSquadsUpdate.OnNext(selectedSquads);
            }

            _squadUpdate.OnNext(squadModel);
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

        //private bool ValidateSquad(SquadModel squadModel)
        //{
        //    var squadData = squadModel.Data;
        //    int unitTotal = squadData["Thief"] + squadData["Hacker"] + squadData["Soldier"]
        //        + squadData["Assassin"] + squadData["Fixer"] + squadData["Scientist"];
        //    int ugoTotal = squadData["Ugo Combat"] + squadData["Ugo Stealth"] + squadData["Ugo Cunning"] + squadData["Ugo Diplomacy"];

        //    string validationError = "";

        //    if (unitTotal > MAX_SQUAD_SIZE || unitTotal < 0)
        //    {
        //        validationError += "Invalid squad size\n";
        //    }
        //    if (squadData["Named Henchman"] > NUMBER_OF_FACED_HENCHMAN || squadData["Named Henchman"] < 0)
        //    {
        //        validationError += "Invalid number of named henchmen\n";
        //    }
        //    if (squadData["Hypnotic Spray"] > 1 || squadData["Hypnotic Spray"] < 0)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    if (squadData["Explosive Rounds"] > 1 || squadData["Explosive Rounds"] < 0)
        //    {
        //        validationError += "\n";
        //    }
        //    if (ugoTotal > MAX_SQUAD_SIZE || ugoTotal < 0)
        //    {
        //        validationError += "\n";
        //    }
        //    if (validationError != "")
        //    {
        //        _errorMessage.OnNext(validationError);
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        #endregion
    }
}
