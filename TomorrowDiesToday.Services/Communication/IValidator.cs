using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Communication
{
    public interface IValidator
    {
        Task Send(IValidatable model);

        event EventHandler<IModel> ModelReceived;
    }
}
