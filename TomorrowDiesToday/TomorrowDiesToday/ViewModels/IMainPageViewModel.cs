using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TomorrowDiesToday.ViewModels
{
    public interface IMainPageViewModel
    {
        bool closed { get; }
        ICommand OnPlayerClickedCommand { get; }
    }
}
