using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TomorrowDiesToday.Services.Communication
{
    /// <summary>
    /// When receiving data, new up a PipelineItem, apply PipelineDirection.In to it, and call _pipeline.Process(item).
    /// The pipeline will call the Communicator's Send method, which means the data is ready to be broadcast.
    /// </summary>
    internal class Communicator : ICommunicator
    {
        private IPipeline _pipeline;

        public Communicator(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public async Task Send(string data)
        {
            throw new NotImplementedException();
        }
    }
}
