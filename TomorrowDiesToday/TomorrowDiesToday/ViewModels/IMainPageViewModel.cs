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
        ObservableCollection<PlayerModel> Players { get; }

        ObservableCollection<object> Items { get; }

        ICommand RefreshPlayerListCommand { get; }

        string GameId { get; set; }
    }
}
