using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.ViewModels
{
    interface IJoinGameViewModel
    {
        ICommand JoinGameCommand { get; }
        ICommand NextStepCommand { get; }

        string GameId { get; set; }
        bool IsLoadingData { get; set; }
        bool GameJoined { get; set; }
        bool InvalidGameId { get; set; }
    }
}
