using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    [DynamoDBTable("GameTable")]

    public class SquadRequestDTO : IRequestDTO
    {
        [DynamoDBHashKey]

        public string SquadId { get; set; }

        public string GameId { get; set; }
    }
}
