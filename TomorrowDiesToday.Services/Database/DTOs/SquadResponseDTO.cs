using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    [DynamoDBTable("GameTable")]

    public class SquadResponseDTO : IResponseDTO
    {
        [DynamoDBHashKey]

        public string SquadId { get; set; }

        public string GameId { get; set; }

        public string PlayerId { get; set; }

        public string SquadData { get; set; }
    }
}
