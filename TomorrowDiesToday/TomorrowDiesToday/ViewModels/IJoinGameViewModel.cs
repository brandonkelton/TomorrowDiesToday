using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.ViewModels
{
    public interface IJoinGameViewModel
    {
        ICommand JoinGameCommand { get; }

        string GameId { get; set; }
        bool IsLoadingData { get; }
        bool InvalidGameId { get; }
    }
}
