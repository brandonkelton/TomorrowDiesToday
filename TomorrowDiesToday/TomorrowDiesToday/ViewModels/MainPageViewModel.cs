using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using Xamarin.Forms;

namespace TomorrowDiesToday.ViewModels
{
    public class MainPageViewModel : BaseViewModel, IMainPageViewModel
    {
        public MainPageViewModel()
        {
            Items = new ObservableCollection<object>
            {
                new {Title="First", ComReq=5, SteReq=6,CunReq=1,DipReq=2 },
                new {Title="second", ComReq=5, SteReq=6,CunReq=1,DipReq=2},
                new {Title="third", ComReq=5, SteReq=6,CunReq=1,DipReq=2}
            };
        }
        public ObservableCollection<object> Items { get; }


    }
}
