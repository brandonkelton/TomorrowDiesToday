using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication
{
    internal class PipelineItemStatusResult
    {
        public PipelineItemStatusResult(PipelineItemStatus status, string message = null)
        {
            Status = status;
            Message = message;
        }

        public PipelineItemStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
