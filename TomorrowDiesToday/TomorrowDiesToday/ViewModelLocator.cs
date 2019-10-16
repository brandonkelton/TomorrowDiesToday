using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.ViewModels;

namespace TomorrowDiesToday
{
    public class ViewModelLocator
    {
        public IMainPageViewModel MainPageViewModel => IoC.Container.Resolve<IMainPageViewModel>();
        public IStartPageViewModel StartPageViewModel => IoC.Container.Resolve<IStartPageViewModel>();
        public ICreateGameViewModel CreateGameViewModel => IoC.Container.Resolve<ICreateGameViewModel>();
        public IJoinGameViewModel JoinGameViewModel => IoC.Container.Resolve<IJoinGameViewModel>();
        public ISelectCharacterViewModel SelectCharacterViewModel => IoC.Container.Resolve<ISelectCharacterViewModel>();
        public IWaitForPlayersViewModel WaitForPlayersViewModel => IoC.Container.Resolve<IWaitForPlayersViewModel>();
    }
}
