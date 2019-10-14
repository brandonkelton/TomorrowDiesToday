using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using TomorrowDiesToday.Models;
using System.Windows.Input;

namespace TomorrowDiesToday.ViewModels
{
    interface ICreateGameViewModel
    {
        //ObservableCollection<PlayerModel> Players { get; }

        ICommand CreateGameCommand { get; }
        //ICommand SetIsJoiningGameCommand { get; }
        //ICommand JoinGameCommand { get; }
        ICommand NextStepCommand { get; }
        //ICommand CreatePlayerCommand { get; }
        //ICommand RefreshPlayerListCommand { get; }
        //ICommand ConfigureTableCommand { get; }
        //ICommand EncryptCommand { get; }

        string GameId { get; set; }
        bool IsLoadingData { get; set; }
        //bool IsWaitingForSelection { get; set; }
        //bool IsCreatingOrJoiningGame { get; set; }
        //bool IsCreatingGame { get; set; }
        //bool IsJoiningGame { get; set; }
        bool GameCreated { get; set; }
        //bool GameJoined { get; set; }
        //bool IsSelectingPlayers { get; set; }
        //bool InvalidGameId { get; set; }
        //bool PlayerExists { get; set; }
        //bool IsWaitingForPlayers { get; set; }
    }
}
