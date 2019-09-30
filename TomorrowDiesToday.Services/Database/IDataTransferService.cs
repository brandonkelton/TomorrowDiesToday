using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Services.Database.DTOs;

namespace TomorrowDiesToday.Services.Database
{
    public interface IDataTransferService<T, U, V> where T : IRequestDTO where U : IResponseDTO where V : IUpdateDTO
    {
        event EventHandler<U> ResponseReceived;

        Task Request(T squad);

        Task Update(V squad);
    }
}
