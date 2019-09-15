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

        private ICommunicator _communicator;
        private IDataService _dataService;

        public Pipeline(ICommunicator communicator, IDataService dataService)
        {
            _communicator = communicator;
            _dataService = dataService;
        }

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
                // This is rough around the edges; revisit
                _dataService.HandleDataReceived(item.Data as IModel);
            }
            else if (result.Direction == PipelineDirection.Out)
            {
                // This is rough around the edges; revisit
                _communicator.Send(item.Data as string);
            }
        }
    }
}
