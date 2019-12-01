using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data.Models;
using TomorrowDiesToday.Services.Database.DTOs;

namespace TomorrowDiesToday.Services.Data
{
    public interface IDataService<T, U> where T : IModel where U : IDataRequest
    {
        IObservable<T> DataReceived { get; }
        IObservable<List<T>> DataListReceived { get; }

        Task ConfigureTable();

        Task Create(T model);

        Task<bool> Exists(U request);

        Task RequestUpdate(U request);

        Task Update(T model);
    }
}
