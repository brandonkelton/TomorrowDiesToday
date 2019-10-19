using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using Xamarin.Forms;

namespace TomorrowDiesToday.Services.Navigation
{
    public interface IViewLocator
    {
        Page CreateAndBindPageFor<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase;
    }
}
