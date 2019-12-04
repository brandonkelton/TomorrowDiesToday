using Autofac;
using System.Threading.Tasks;
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Services.Database;
using TomorrowDiesToday.Services.Game;
using TomorrowDiesToday.Services.LocalStorage;
using Xamarin.Forms;

namespace TomorrowDiesToday
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
        }

        protected override void OnStart()
        {
            IoC.Initialize();
            var navigationService = IoC.Container.Resolve<INavigationService>();
            MainPage = navigationService.Navigation;
        }

        protected override void OnSleep()
        {
            var storageService = IoC.Container.Resolve<ILocalStorageService>();
            var dbClient = IoC.Container.Resolve<IDBClient>();
            var gameState = IoC.Container.Resolve<IGameState>();

            Task.Run(async () =>
            {
                if (gameState.Game != null && gameState.Game.GameId != null && gameState.Game.PlayerId != null)
                {
                    await storageService.SaveGame();
                    await dbClient.DeleteGame(gameState.Game.GameId, gameState.Game.PlayerId);
                }
            }).Wait();
        }

        protected override void OnResume()
        {
            var storageService = IoC.Container.Resolve<ILocalStorageService>();
            Task.Run(async () =>
            {
                await storageService.LoadGame();
            }).Wait();
        }

        protected override void CleanUp()
        {
            IoC.Destroy();
            base.CleanUp();
        }
    }
}
