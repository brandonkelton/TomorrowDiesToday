using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    [DynamoDBTable("Games")]
    public class GameDTO
    {
        [DynamoDBHashKey]
        public string GameId { get; set; }

        public List<TileDTO> Tiles { get; set; } = new List<TileDTO>();
    }
}
