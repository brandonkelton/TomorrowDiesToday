using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    /// <summary>
    /// This is implemented as a List item within the Games table.
    /// </summary>
    public class TileDTO
    {
        public int AlertTokens { get; set; }

        public bool IsActive { get; set; }

        public bool IsAgentCIA { get; set; }

        public bool IsAgentInterpol { get; set; }

        public bool IsDoomsday { get; set; }

        public bool IsFlipped { get; set; }

        public bool IsGlobalSecurityEvent { get; set; }

        public bool IsHQ { get; set; }

        public string TileId { get; set; }
    }
}
