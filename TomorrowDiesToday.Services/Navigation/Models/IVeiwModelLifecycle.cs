using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TomorrowDiesToday.Services.Navigation.Models
{
    public interface IViewModelLifecycle
    {
        Task BeforeFirstShown();
        Task AfterDismissed();
    }
}
