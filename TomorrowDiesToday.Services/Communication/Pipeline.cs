using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Communication
{
    internal class Pipeline : IPipeline
    {
        private List<IPipelineService> _inServices = new List<IPipelineService>();
        private List<IPipelineService> _outServices = new List<IPipelineService>();

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
            PipelineItem lastResult = item;
            foreach (var service in services)
            {
                service.Process(item);

            }
        }
    }
}
