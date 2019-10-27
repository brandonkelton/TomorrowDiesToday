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
        #region SideSheet
        private ConstraintExpression _sideSheetX;
        public ConstraintExpression sideSheetX
        {
            get => _sideSheetX;
            set => SetProperty(ref _sideSheetX, value);
        }

        private ConstraintExpression _sideSheetY;
        public ConstraintExpression sideSheetY
        {
            get => _sideSheetY;
            private set => SetProperty(ref _sideSheetY, value);
        }

        private ConstraintExpression _sideSheetHeight;
        public ConstraintExpression sideSheetHeight
        {
            get => _sideSheetHeight;
            private set => SetProperty(ref _sideSheetHeight, value);
        }

        private ConstraintExpression _sideSheetWidth;
        public ConstraintExpression sideSheetWidth
        {
            get => _sideSheetWidth;
            private set => SetProperty(ref _sideSheetWidth, value);
        }

        private bool _closed;
        public bool closed
        {
            get => _closed;
            private set => SetProperty(ref _closed, value);
        }
        #endregion

        private double _sideSheetHeightFactor;
        public double sideSheetHeightFactor
        {
            get => _sideSheetHeightFactor;
            private set => SetProperty(ref _sideSheetHeightFactor, value);
        }

        public string title => "Tomorrow Dies Today";
        public ICommand OnPlayerClickedCommand { get; private set; }

        public MainPageViewModel()
        {
            //sideSheetHeightFactor = 1;
            ConfigureCommands();
        }

        private void ConfigureCommands()
        {
            OnPlayerClickedCommand = new Command(() => OnPlayerClicked());
        }

        private void OnPlayerClicked()
        {
            if (closed)
            {
                //sideSheet.TranslateTo(Width * .75, sideSheet.Y, 100);
                closed = false;
            }
            else
            {
                //sideSheet.TranslateTo(0, sideSheet.Y, 100);
                closed = true;
            }
        }
    }
}
