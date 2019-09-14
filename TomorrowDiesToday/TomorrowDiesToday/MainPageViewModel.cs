using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Services.Communication;
using Xamarin.Forms;

namespace TomorrowDiesToday
{
    class MainPageViewModel
    {
        private IDataService _dataService;

        public MainPageViewModel()
        {
            _dataService = DependencyService.Get<IDataService>();
        }
    }
}
