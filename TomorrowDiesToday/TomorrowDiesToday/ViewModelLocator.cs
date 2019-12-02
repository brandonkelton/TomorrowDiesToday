using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.ViewModels;

namespace TomorrowDiesToday
{
    public class ViewModelLocator
    {
        public MainPageViewModel MainPageViewModel => IoC.Container.Resolve<MainPageViewModel>();
        public StartPageViewModel StartPageViewModel => IoC.Container.Resolve<StartPageViewModel>();
        public CreateGameViewModel CreateGameViewModel => IoC.Container.Resolve<CreateGameViewModel>();
        public JoinGameViewModel JoinGameViewModel => IoC.Container.Resolve<JoinGameViewModel>();
        public SelectCharacterViewModel SelectCharacterViewModel => IoC.Container.Resolve<SelectCharacterViewModel>();
        public WaitForPlayersViewModel WaitForPlayersViewModel => IoC.Container.Resolve<WaitForPlayersViewModel>();
        public ResumeGameViewModel ResumeGameViewModel => IoC.Container.Resolve<ResumeGameViewModel>();
    }
}
