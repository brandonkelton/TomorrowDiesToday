using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.ViewModels
{
    public interface IMainPageViewModel
    {
        ObservableCollection<object> Items { get; }

        ObservableCollection<PlayerModel> Players { get; }

        ObservableCollection<PlayerModel> ThisPlayer { get; }

        ICommand RefreshPlayerListCommand { get; }

        ICommand ToggleSelectedSquadCommand { get; }

        ICommand SelectSquad1Command { get; }

        ICommand SelectSquad2Command { get; }

        ICommand SelectSquad3Command { get; }

        string GameId { get; }

        bool Squad1Selected { get; }

        bool Squad2Selected { get; }

        bool Squad3Selected { get; }

        bool Squad1Selectable { get; }

        bool Squad2Selectable { get; }

        bool Squad3Selectable { get; }
    }
}
