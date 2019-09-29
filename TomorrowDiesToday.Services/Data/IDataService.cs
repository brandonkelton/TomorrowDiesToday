using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Data
{
    public interface IDataService<T, U> where T : IModel where U : IDataRequest
    {
        void Send(T model);

        IObservable<T> Update { get; }

        void RequestUpdate(U request);
    }
}
