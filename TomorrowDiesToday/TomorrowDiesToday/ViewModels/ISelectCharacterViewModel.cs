using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.ViewModels
{
    public interface ISelectCharacterViewModel
    {
        ObservableCollection<PlayerModel> Players { get; }

        ICommand NextStepCommand { get; }
        ICommand CreatePlayerCommand { get; }

        string GameId { get; set; }
        bool IsLoadingData { get; set; }
        bool PlayerExists { get; set; }
    }
}
