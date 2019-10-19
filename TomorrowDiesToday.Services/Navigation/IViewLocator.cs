using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Services.Navigation.Models;
using Xamarin.Forms;

namespace TomorrowDiesToday.Services.Navigation
{
    public interface IViewLocator
    {
        Page CreateAndBindPageFor<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase;
    }
}
