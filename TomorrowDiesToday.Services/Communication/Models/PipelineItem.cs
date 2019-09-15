using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication
{
    public class PipelineItem
    {
        public PipelineItem(PipelineDirection direction, object data)
        {
            Direction = direction;
            Data = data;
        }

        public PipelineDirection Direction { get; private set; }

        public object Data { get; private set; }

        public PipelineItemStatusResult StatusResult { get; private set; }

        public void SetStatusResult(PipelineItemStatusResult statusResult)
        {
            StatusResult = statusResult;
        }
    }
}
