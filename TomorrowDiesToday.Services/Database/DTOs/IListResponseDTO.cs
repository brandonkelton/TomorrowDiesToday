using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    public interface IListResponseDTO<T> where T : IResponseDTO
    {
        List<T> Items { get; }
    }
}
