using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TomorrowDiesToday.ViewModels
{
    public interface IViewModelLifecycle
    {
        Task BeforeFirstShown();
        Task AfterDismissed();
    }
}
