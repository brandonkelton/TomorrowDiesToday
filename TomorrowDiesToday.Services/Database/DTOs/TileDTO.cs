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
    }
}
