﻿using System;
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
            OnPlayerClicked();
            
        }
        double x, y;
        Boolean open = false;
        
        void OnPanUpdated(object sender, PanUpdatedEventArgs e) 
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    var translateY = Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs((Height * .25) - Height));
                    bottomSheet.TranslateTo(bottomSheet.X, translateY, 30);
                    break;
                case GestureStatus.Completed:
                    y = bottomSheet.TranslationY;

                    var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(getClosestLockState(e.TotalY + y)));



                    if (e.TotalY < 0)
                    {
                        bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringIn);
                    }
                    else
                    {
                        bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringOut);
                    }
                    y = bottomSheet.TranslationY;

                    break;

            }
             double getClosestLockState(double TranslationY)
            {
                var lockStates = new double[] { 0, .5, .85 };
                var distance = Math.Abs(TranslationY);
                var currentProportion = distance / Height;

                var smallestDistance = 10000.0;
                var closestIndex = 0;
                for (var i = 0; i < lockStates.Length; i++)
                {
                    var state = lockStates[i];
                    var absoluteDistance = Math.Abs(state - currentProportion);
                    if (absoluteDistance < smallestDistance)
                    {
                        smallestDistance = absoluteDistance;
                        closestIndex = i;
                    }
                }

                var selectedLockState = lockStates[closestIndex];
                var TranslateToLockState = selectedLockState * Height;

                return TranslateToLockState;
            }
            
            
        }
        void OnPlayerClicked(object sender, EventArgs e)
        {
            
            if (open)
            {
                sideSheet.TranslateTo(Width * .75, sideSheet.Y, 100);

                open = false;
            }
            else
            {
                sideSheet.TranslateTo(Width * .75, sideSheet.Y);
                sideSheet.IsVisible = true;
                sideSheet.TranslateTo(0, sideSheet.Y, 100);
                open = true;
            }
        }
        void OnPlayerClicked()
        {

            if (open)
            {
                sideSheet.TranslateTo(Width * .75, sideSheet.Y, 100);

                open = false;
            }
            else
            {
                sideSheet.TranslateTo(Width * .75, sideSheet.Y);
                sideSheet.IsVisible = true;
                sideSheet.TranslateTo(0, sideSheet.Y, 100);
                open = true;
            }
        }
    }
}
