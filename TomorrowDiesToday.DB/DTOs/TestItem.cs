using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.DB.DTOs
{
    [DynamoDBTable("TestTable")]
    public class TestItem
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Category { get; set; }

        public string Text { get; set; }
    }
}
