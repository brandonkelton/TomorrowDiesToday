using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.DB;
using TomorrowDiesToday.DB.DTOs;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Communication;
using TomorrowDiesToday.Services.Store;
using Xamarin.Forms;

namespace TomorrowDiesToday.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<TestItem> SearchResults { get; private set; } = new ObservableCollection<TestItem>();
        public ICommand DeleteCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }
        public ICommand SendCommand { get; private set; }
        public ICommand ReceiveCommand { get; private set; }

        private DynamoClient _client = new DynamoClient();

        private string _sendText;
        public string SendText
        {
            get { return _sendText; }
            set { SetProperty(ref _sendText, value); }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }

        public List<string> Categories
        {
            get => new List<string>
            {
                "Player 1",
                "Player 2",
                "Player 3",
                "Player 4"
            };
    }

        private string _receivedText;
        public string ReceivedText
        {
            get { return _receivedText; }
            set { SetProperty(ref _receivedText, value); }
        }

        public MainPageViewModel()
        {
            SubscribeToEvents();
            SendCommand = new Command(async () => await _client.Send(SendText, SelectedCategory));
            ReceiveCommand = new Command(async () => await _client.Receive());
            CreateCommand = new Command(async () => await _client.Initialize());
            DeleteCommand = new Command(async () => await _client.DeleteTable());

            _client.SearchResultReceived += OnSearchResultReceived;
        }

        private void OnSearchResultReceived(object sender, List<TestItem> items)
        {
            SearchResults.Clear();
            items.ForEach(item => SearchResults.Add(item));
        }

        private void SubscribeToEvents()
        {
            // The store is on the BaseViewModel, since all view models will need access to data.
            // This event implementation is specific to this view model, as each view model may need different data.
            //_store.HasUpdatedModel += OnUpdatedModel;
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
