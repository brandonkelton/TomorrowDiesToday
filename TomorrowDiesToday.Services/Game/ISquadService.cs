using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface ISquadService
    {
        IObservable<string> ErrorMessage { get; }

        IObservable<List<SquadModel>> SelectedSquadsUpdate { get; }

        IObservable<SquadStats> SelectedSquadStatsUpdate { get; }

        IObservable<SquadModel> SquadUpdate { get; }

        void CalculateSquadStats(SquadModel squadModel);

        void DecrementAbilityCount(ArmamentType abilityType, SquadModel squadModel);

        void DecrementArmamentCount(ArmamentType armamentType, SquadModel squadModel);

        void DecrementItemCount(ArmamentType itemType, SquadModel squadModel);

        void IncrementAbilityCount(ArmamentType abilityType, SquadModel squadModel);

        void IncrementArmamentCount(ArmamentType armamentType, SquadModel squadModel);

        void IncrementItemCount(ArmamentType itemType, SquadModel squadModel);

        void ToggleSelected(SquadModel squadModel);
    }
}
