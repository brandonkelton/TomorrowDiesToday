using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Communication
{
    public interface IPipeline
    {
        IObservable<IModel> InRequest { get; }

        IObservable<string> OutRequest { get; }

        void AddService(PipelineDirection direction, IPipelineService service);

        void Process(PipelineItem item);
    }
}
