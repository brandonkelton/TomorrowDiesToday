using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    [DynamoDBTable("PlayerData")]
    public class PlayerDTO
    {
        [DynamoDBHashKey]
        public string GameId { get; set; }

        [DynamoDBRangeKey]
        public string PlayerId { get; set; }

        public List<SquadDTO> Squads { get; set; }
    }
}
