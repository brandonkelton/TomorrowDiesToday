using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Communication.Exceptions;

namespace TomorrowDiesToday.Services.Communication
{
    internal class Pipeline : IPipeline
    {
        private List<IPipelineService> _inServices = new List<IPipelineService>();
        private List<IPipelineService> _outServices = new List<IPipelineService>();

        public event EventHandler<string> OutRequest;
        public event EventHandler<IModel> InRequest;

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
            }

            if (result.Direction == PipelineDirection.In)
            {
                InRequest?.Invoke(this, item.Data as IModel);
            }
            else if (result.Direction == PipelineDirection.Out)
            {
                OutRequest?.Invoke(this, item.Data as string);
            }
        }
    }
}
