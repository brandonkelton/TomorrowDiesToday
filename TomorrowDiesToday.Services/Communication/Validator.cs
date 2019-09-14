using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Communication
{
    /// <summary>
    /// Listens to the communicator's DataReceived event, then validates the data and converts it from JSON to a model.
    /// Then emits a new DataReceived event containing the new model.
    /// You'll have to somehow test the data to determine what kind of model it is.  Currently there is only GameModel, but there will be others.
    /// </summary>
    public class Validator : IValidator
    {
        public event EventHandler<IModel> ModelReceived;
        private ICommunicator _communicator;

        public Validator()
        {
            
        }

        public Task Send(IValidatable model)
        {
            throw new NotImplementedException();
            
        }

        private void Example(string data)
        {
            var model = JsonConvert.DeserializeObject<GameModel>(data);
            var serialized = JsonConvert.SerializeObject(model);
        }
    }
}
