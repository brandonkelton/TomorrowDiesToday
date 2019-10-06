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
        Task<bool> Exists(T model);

        Task Create(T model);

        Task Update(T model);

        IObservable<T> DataReceived { get; }

        Task RequestUpdate(U request);
    }
}
