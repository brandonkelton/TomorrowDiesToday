using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Models.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TomorrowDiesToday.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TDTNavigationPage : NavigationPage
    {
        public TDTNavigationPage()
        {
            InitializeComponent();
        }
    }
}