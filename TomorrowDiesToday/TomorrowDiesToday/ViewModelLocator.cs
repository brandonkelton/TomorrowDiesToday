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
    }
}
