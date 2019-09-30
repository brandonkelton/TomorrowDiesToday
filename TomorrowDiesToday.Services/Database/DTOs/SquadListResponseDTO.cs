using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    public class SquadListResponseDTO : IListResponseDTO<SquadResponseDTO>, IResponseDTO
    {
        public string GameId { get; set; }

        public List<SquadResponseDTO> Items { get; } = new List<SquadResponseDTO>();
    }
}
