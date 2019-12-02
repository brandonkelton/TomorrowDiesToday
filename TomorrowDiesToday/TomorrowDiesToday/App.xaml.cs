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
            var gameService = IoC.Container.Resolve<IGameService>();

            Task.Run(async () =>
            {
                if (gameService.Game != null && gameService.Game.GameId != null && gameService.Game.PlayerId != null)
                {
                    await storageService.SaveGame();
                    await dbClient.DeleteGame(gameService.Game.GameId, gameService.Game.PlayerId);
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
