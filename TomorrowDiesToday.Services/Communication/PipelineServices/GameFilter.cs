using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication.PipelineServices
{
    /// <summary>
    /// This will validate whether or not a broadcast is relevant for the current game, maybe using a Guid in the model to identify the current game.
    /// Ex: If other games are nearby, we don't want to allow their data into our game model.
    /// </summary>
    internal class GameFilter : IPipelineService
    {
        public PipelineItem Result => throw new NotImplementedException();

        public void Process(PipelineItem item)
        {
            throw new NotImplementedException();
        }
    }
}
