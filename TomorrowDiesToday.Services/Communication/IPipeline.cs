using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Communication
{
    public interface IPipeline
    {
        event EventHandler<string> OutRequest;

        event EventHandler<IModel> InRequest;

        void AddService(PipelineDirection direction, IPipelineService service);

        void Process(PipelineItem item);
    }
}
