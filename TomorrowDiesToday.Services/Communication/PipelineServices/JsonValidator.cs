using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication.PipelineServices
{
    /// <summary>
    /// This validates that the text the communicator produces is valid JSON
    /// </summary>
    internal class JsonValidator : IPipelineService
    {
        public PipelineItem Result => throw new NotImplementedException();

        public void Process(PipelineItem item)
        {
            throw new NotImplementedException();
        }
    }
}
