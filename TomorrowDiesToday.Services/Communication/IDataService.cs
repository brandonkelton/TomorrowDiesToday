using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Communication
{
    public interface IDataService
    {
        event EventHandler<IModel> DataReceived;

        Task Send(IModel model);

        void HandleDataReceived(IModel model);
    }
}
