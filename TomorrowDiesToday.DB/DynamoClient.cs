using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TomorrowDiesToday.DB.DTOs;

namespace TomorrowDiesToday.DB
{
    public class DynamoClient
    {
        public event EventHandler<List<TestItem>> SearchResultReceived;

        private string GameName = "TDT - Game 1";

        public async Task DeleteTable()
        {
            var credentials = new TDTCredentials();
            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);

            var tables = (await client.ListTablesAsync()).TableNames;
            if (tables.Contains("TestTable"))
            {
                var request = new DeleteTableRequest("TestTable");
                var response = await client.DeleteTableAsync(request);
                Console.Write(response.HttpStatusCode.ToString());
            }
        }

        public async Task Initialize()
        {
            var credentials = new TDTCredentials();
            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);

            var tables = (await client.ListTablesAsync()).TableNames;
            if (!tables.Contains("TestTable"))
            {
                var request = new CreateTableRequest
                {
                    TableName = "TestTable",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "Id",
                            AttributeType = "S"
                        },
                        new AttributeDefinition
                        {
                            AttributeName = "Category",
                            AttributeType = "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = "HASH"
                        },
                        new KeySchemaElement
                        {
                            AttributeName = "Category",
                            KeyType = "RANGE"
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                };

                var response = await client.CreateTableAsync(request);

                Console.WriteLine(response.HttpStatusCode.ToString());
            }
            
        }

        public async Task Send(string text, string category)
        {
            var credentials = new TDTCredentials();
            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);
            var context = new DynamoDBContext(client);
            var item = new TestItem
            {
                Id = GameName,
                Category = category,
                Text = text
            };
            await context.SaveAsync(item);
        }

        public async Task Receive()
        {
            var credentials = new TDTCredentials();
            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);
            var context = new DynamoDBContext(client);
            var search = context.QueryAsync<TestItem>(GameName);
            var results = await search.GetRemainingAsync();

            SearchResultReceived(this, results);
        }
    }
}
