using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication.PipelineServices
{
    /// <summary>
    /// This converts the data returned by the Communicator into a actual model.
    /// TIP:  Use JsonConvert.Deserialize<T>(data)
    /// </summary>
    internal class DataDeserializer : IPipelineService
    {
        public PipelineItem Result => throw new NotImplementedException();

        public void Process(PipelineItem item)
        {
            throw new NotImplementedException();
        }
    }
}
