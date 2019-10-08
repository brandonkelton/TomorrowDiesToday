using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    /// <summary>
    /// This is implemented as a List item within the Players table.
    /// </summary>
    public class SquadDTO
    {
        public string SquadId { get; set; }
        public Dictionary<string, int> Data { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<string, int> Stats { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
