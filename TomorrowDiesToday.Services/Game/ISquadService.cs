﻿using System;
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

        string[] NamedHenchmenNames { get; }
        Dictionary<string, int[]> NamedHenchmenStats { get; }

        void CalculateSquadStats(SquadModel squadModel);
        void ToggleSquad(SquadModel squadModel);
    }
}
