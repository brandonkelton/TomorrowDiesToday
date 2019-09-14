using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Communication
{
    /// <summary>
    /// This service will look at the model, compare its dates against its local version, and decide what to keep and integrate into its local version.
    /// It may drop the data all-together, if the dates are all older than its local version, it may integrate only part of the model,
    /// basically there are integration decisions to make here.  It may also convert view-model data into data transfer objects because they will probably differ.
    /// </summary>
    public interface IDataService
    {
        Task Send(IModel model);
    }
}
