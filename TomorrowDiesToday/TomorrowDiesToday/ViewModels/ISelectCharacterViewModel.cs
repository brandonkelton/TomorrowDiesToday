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
        ICommand SelectPlayerCommand { get; }

        string GameId { get; }
        bool IsLoadingData { get; }
        bool PlayerExists { get; }
        string PlayerAlreadySelected { get; }
    }
}
