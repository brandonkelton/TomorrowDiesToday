using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TomorrowDiesToday.Models
{
    public interface IViewModelLifecycle
    {
        Task BeforeFirstShown();
        Task AfterDismissed();
    }
}
