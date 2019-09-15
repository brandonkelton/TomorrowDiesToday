using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication.PipelineServices
{
    /// <summary>
    /// This will validate whether or not a broadcast is relevant for the current active game, maybe using a Guid in the received model
    /// </summary>
    internal class GameValidator : IPipelineService
    {
        public PipelineItem Result => throw new NotImplementedException();

        public void Process(PipelineItem item)
        {
            throw new NotImplementedException();
        }
    }
}
