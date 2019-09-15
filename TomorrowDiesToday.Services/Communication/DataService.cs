using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Communication
{
    /// <summary>
    /// This service will look at the model, compare its dates against its local version, and decide what to keep and integrate into its local version.
    /// It may drop the data altogether, if the dates are older than its local version, it may integrate only part of the model,
    /// basically there are integration decisions to make here.  It may also convert view-model data into data transfer objects because they will probably differ.
    /// This design is tentative, but I'm thinking the main project will access this service and send data using a Send() method, and listen for the
    /// DataReceived event to fire.
    /// NOTE: The Send() method may only need to new up a PipelineItem, apply PipelineDirection.Out and call _pipeline.Process(item).
    /// </summary>
    public class DataService : IDataService
    {
        public event EventHandler<IModel> DataReceived;
        private IPipeline _pipeline;

        public DataService(IPipeline pipeline)
        {
            _pipeline = pipeline;
            _pipeline.InRequest.Subscribe(model => HandleDataReceived(model));
        }

        public Task Send(IModel model)
        {
            throw new NotImplementedException();
        }

        private void HandleDataReceived(IModel model)
        {
            DataReceived?.Invoke(this, model);
        }
    }
}
