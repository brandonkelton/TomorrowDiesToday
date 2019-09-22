using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Store
{
    public interface IDataStore
    {
        event EventHandler<IModel> HasUpdatedModel;
    }
}
