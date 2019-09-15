using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication
{
    public interface IPipelineService
    {
        void Process(PipelineItem item);

        PipelineItem Result { get; }
    }
}
