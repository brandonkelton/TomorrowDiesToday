using Autofac;
using System;
using System.Threading.Tasks;
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Services;
using TomorrowDiesToday.Services.LocalStorage;
using TomorrowDiesToday.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TomorrowDiesToday
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            IoC.Initialize();
            var navigationService = IoC.Container.Resolve<INavigationService>();
            MainPage = navigationService.Navigation;
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
            //var localStorage = IoC.Container.Resolve<ILocalStorageService>();
            //localStorage.StoreGame();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void CleanUp()
        {
            IoC.Destroy();
            base.CleanUp();
        }

        //#region IDisposable Support
        //private bool disposedValue = false; // To detect redundant calls

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            var localStorage = IoC.Container.Resolve<ILocalStorageService>();
        //            localStorage.StoreGame();
        //        }

        //        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        //        // TODO: set large fields to null.

        //        disposedValue = true;
        //    }
        //}

        //// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        //// ~GameService()
        //// {
        ////   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        ////   Dispose(false);
        //// }

        //// This code added to correctly implement the disposable pattern.
        //public void Dispose()
        //{
        //    // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //    Dispose(true);
        //    // TODO: uncomment the following line if the finalizer is overridden above.
        //    // GC.SuppressFinalize(this);
        //}
        //#endregion
    }
}
