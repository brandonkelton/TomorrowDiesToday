using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication
{
    internal interface IPipelineService
    {
        void Process(PipelineItem item);

        PipelineItem Result { get; }
    }
}
