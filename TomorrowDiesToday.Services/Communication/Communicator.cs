using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TomorrowDiesToday.Services.Communication
{
    public class Communicator : ICommunicator
    {
        public event EventHandler<string> DataReceived;

        public async Task Send(string data)
        {
            throw new NotImplementedException();
        }
    }
}
