using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TomorrowDiesToday.Services.Communication
{
    public interface ICommunicator
    {
        Task Send(string data);

        event EventHandler<string> DataReceived;
    }
}
