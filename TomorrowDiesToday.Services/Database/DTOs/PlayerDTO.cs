using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    [DynamoDBTable("PlayerData")]
    public class PlayerDTO
    {
        [DynamoDBHashKey]
        public string GameId { get; set; }

        [DynamoDBRangeKey]
        public ArmamentType PlayerId { get; set; }

        public List<SquadDTO> Squads { get; set; }
    }
}
