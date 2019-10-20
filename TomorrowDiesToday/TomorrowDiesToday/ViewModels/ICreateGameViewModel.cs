﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using TomorrowDiesToday.Models;
using System.Windows.Input;

namespace TomorrowDiesToday.ViewModels
{
    public interface ICreateGameViewModel
    {
        ICommand NextStepCommand { get; }

        string GameId { get; set; }
        bool IsLoadingData { get; set; }
        bool GameCreated { get; set; }
    }
}