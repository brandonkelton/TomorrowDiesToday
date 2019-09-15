using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication
{
    public interface IPipeline
    {
        void AddService(PipelineDirection direction, IPipelineService service);

        void Process(PipelineItem item);
    }
}
