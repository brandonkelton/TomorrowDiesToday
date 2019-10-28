using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TomorrowDiesToday.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }
        Boolean closed = true;
        
        void OnPanUpdated(object sender, PanUpdatedEventArgs e) 
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    var translateY = Math.Max(-Math.Abs(Height * .5), Math.Min(bottomSheet.TranslationY + e.TotalY, 0));
                    bottomSheet.TranslateTo(bottomSheet.X, translateY, 30);
                    break;
                case GestureStatus.Completed:
                    //y = bottomSheet.TranslationY;
                    var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(Height * .5));
                    if (bottomSheet.TranslationY + e.TotalY > -Math.Abs(Height * .5)/2)
                    {
                        finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(0));
                    }
                    
                    if (e.TotalY < 0)
                    {
                        bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringIn);
                    }
                    else
                    {
                        bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringOut);
                    }
                    break;

            }      
        }
        void SwipeBottom(object sender, SwipedEventArgs e)
        {
            var finalTranslation = 0.0;
            switch (e.Direction)
            {
                case SwipeDirection.Up:
                    finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(Height * .5));
                    bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringOut);
                    break;
                case SwipeDirection.Down:
                    finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(0));
                    bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringOut);
                    break;
            }
        }

        void OnPlayerClicked(object sender, EventArgs e)
        {
            
            if (closed)
            {
                sideSheet.TranslateTo(Width * .75, sideSheet.Y, 100);

                closed = false;
            }
            else
            {
                sideSheet.TranslateTo(0, sideSheet.Y, 100);
                closed = true;
            }
        }

        void OnPanUpdatedSide(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    var translateX = Math.Min(Width * .75, Math.Max(sideSheet.TranslationX + e.TotalX, 0));
                    sideSheet.TranslateTo( translateX, sideSheet.Y, 30);
                    break;
                case GestureStatus.Completed:
                    if(sideSheet.TranslationX > (Width * .75) / 2)
                    {
                        sideSheet.TranslateTo(Width * .75, sideSheet.Y, 100);

                        closed = false;
                    }
                    else
                    {
                        sideSheet.TranslateTo(0, sideSheet.Y, 100);
                        closed = true;
                    }
                    break;
                  

            }
        }


    }
}
