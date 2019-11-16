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

        IObservable<SquadModel> SquadUpdate { get; }

        void CalculateSquadStats(SquadModel squadModel);

        void ToggleSelected(SquadModel squadModel);
    }
}
