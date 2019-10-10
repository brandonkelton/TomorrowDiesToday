using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace TomorrowDiesToday.DB.DTOs
{
    [DynamoDBTable("GameTable")]

    public class SquadRequestDTO : IModelDTO
    {
        [DynamoDBHashKey]

        public string SquadId { get; set; }

        public string GameId { get; set; }
    }
}
