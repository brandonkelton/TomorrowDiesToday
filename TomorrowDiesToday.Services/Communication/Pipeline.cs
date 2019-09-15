using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Communication.Exceptions;

namespace TomorrowDiesToday.Services.Communication
{
    internal class Pipeline : IPipeline
    {
        private List<IPipelineService> _inServices = new List<IPipelineService>();
        private List<IPipelineService> _outServices = new List<IPipelineService>();
        private Subject<IModel> _inRequest = new Subject<IModel>();
        private Subject<string> _outRequest = new Subject<string>();

        public IObservable<IModel> InRequest => _inRequest as IObservable<IModel>;
        public IObservable<string> OutRequest => _outRequest as IObservable<string>;

        public void AddService(PipelineDirection direction, IPipelineService service)
        {
            if (direction == PipelineDirection.In)
            {
                _inServices.Add(service);
            }
            else
            {
                _outServices.Add(service);
            }
        }

        public void Process(PipelineItem item)
        {
            var services = item.Direction == PipelineDirection.In ? _inServices : _outServices;
            PipelineItem result = item;
            foreach (var service in services)
            {
                service.Process(result);
                result = service.Result;
                if (result.StatusResult.Status == PipelineItemStatus.Fail)
                {
                    throw new PipelineException(result.StatusResult.Message);
                }
                else if (result.StatusResult.Status == PipelineItemStatus.Drop)
                {
                    break;
                }
            }

            if (item.StatusResult.Status == PipelineItemStatus.Success)
            {
                if (result.Direction == PipelineDirection.In)
                {
                    _inRequest.OnNext(item.Data as IModel);
                }
                else if (result.Direction == PipelineDirection.Out)
                {
                    _outRequest.OnNext(item.Data as string);
                }
            }
        }
    }
}
