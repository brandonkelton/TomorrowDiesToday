using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication.PipelineServices
{
    /// <summary>
    /// This serializes models into JSON strings.
    /// TIP:  Use JsonConvert.Serialize(model)
    /// </summary>
    internal class ModelSerializer : IPipelineService
    {
        public PipelineItem Result => throw new NotImplementedException();

        public void Process(PipelineItem item)
        {
            throw new NotImplementedException();
        }
    }
}
