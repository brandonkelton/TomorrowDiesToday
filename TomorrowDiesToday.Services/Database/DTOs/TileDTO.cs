using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    /// <summary>
    /// This is implemented as a List item within the Games table.
    /// </summary>
    public class TileDTO
    {
        public string TileId { get; set; }

        public bool IsActive { get; set; }

        public bool IsFlipped { get; set; }

        public int AlertTokens { get; set; }
    }
}
