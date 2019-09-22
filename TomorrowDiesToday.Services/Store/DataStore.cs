using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Store
{
    public class DataStore : IDataStore
    {
        public event EventHandler<IModel> HasUpdatedModel;
    }
}
