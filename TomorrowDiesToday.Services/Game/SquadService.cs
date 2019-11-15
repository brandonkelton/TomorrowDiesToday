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
        public IObservable<SquadStats> SelectedSquadStatsUpdate => _selectedSquadStatsUpdate;
        public IObservable<SquadModel> SquadUpdate => _squadUpdate;

        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        private readonly ReplaySubject<List<SquadModel>> _selectedSquadsUpdate = new ReplaySubject<List<SquadModel>>(1);
        private readonly ReplaySubject<SquadStats> _selectedSquadStatsUpdate = new ReplaySubject<SquadStats>(1);
        private readonly ReplaySubject<SquadModel> _squadUpdate = new ReplaySubject<SquadModel>(1);

        // Required Service(s)
        private IGameService _gameService;

        // Constants
        private const int MAX_SQUAD_SIZE = 6;
        private const int NUMBER_OF_FACED_HENCHMAN = 9;

        List<SquadModel> _selectedSquads => _gameService.Game.Players.SelectMany(player => player.Squads.Where(squad => squad.IsSelected)).ToList();

        #endregion

        #region Public Methods

        public void CalculateSquadStats(SquadModel squadModel)
        {
            squadModel.Stats.Combat.SetValue(squadModel.Armaments.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Combat.Value));
            squadModel.Stats.Stealth.SetValue(squadModel.Armaments.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Stealth.Value));
            squadModel.Stats.Cunning.SetValue(squadModel.Armaments.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Cunning.Value));
            squadModel.Stats.Diplomacy.SetValue(squadModel.Armaments.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Diplomacy.Value));

            squadModel.Stats.Combat.Value += (squadModel.Abilities.Where(a =>a.Count > 0).Sum(a => a.Count * a.Stats.Combat.Value));
            squadModel.Stats.Stealth.Value += (squadModel.Abilities.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Stealth.Value));
            squadModel.Stats.Cunning.Value += (squadModel.Abilities.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Cunning.Value));
            squadModel.Stats.Diplomacy.Value += (squadModel.Abilities.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Diplomacy.Value));


            if (squadModel.IsSelected)
            {
                var selectedSquads = _gameService.Game.Players.SelectMany(player => player.Squads.Where(squad => squad.IsSelected)).ToList();
                _selectedSquadsUpdate.OnNext(selectedSquads);
            }

            _squadUpdate.OnNext(squadModel);
        }

        public void ToggleSelected(SquadModel squadModel)
        {
            squadModel.IsSelected = !squadModel.IsSelected;
            _selectedSquadsUpdate.OnNext(_selectedSquads);
            SumSelectedSquadStats();
            _squadUpdate.OnNext(squadModel);
        }

        #endregion

        #region Helper Methods

        private void SumSelectedSquadStats()
        {
            SquadStats stats = _gameService.Game.SelectedSquadStats;

            stats.Combat.SetValue(_selectedSquads.Sum(a => a.Stats.Combat.Value));
            stats.Stealth.SetValue(_selectedSquads.Sum(a => a.Stats.Stealth.Value));
            stats.Cunning.SetValue(_selectedSquads.Sum(a => a.Stats.Cunning.Value));
            stats.Diplomacy.SetValue(_selectedSquads.Sum(a => a.Stats.Diplomacy.Value));

            _selectedSquadStatsUpdate.OnNext(stats);
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
