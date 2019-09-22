using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Communication;
using TomorrowDiesToday.Services.Store;
using Xamarin.Forms;

namespace TomorrowDiesToday.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            // The store is on the BaseViewModel, since all view models will need access to data.
            // This event implementation is specific to this view model, as each view model may need different data.
            _store.HasUpdatedModel += OnUpdatedModel;
        }

        // This event implementation is specific to this view model, as each view model may need different data.
        private void OnUpdatedModel(object sender, IModel model)
        {
            // Example - MainPageViewModel may not want to listen to whole model changes, but rather submodel changes
            if (model is GameModel)
            {
                // Update properties for this view model using the provided model
            }
        }
    }
}
