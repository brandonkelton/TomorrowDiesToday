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
        private AmazonDynamoDBClient _client;
        private bool altConfig = false;


        private string GameName = "TDT - Game 1";


        public void ConfigureClient()
        {
            if (altConfig)
            {
                var config = new AmazonDynamoDBConfig
                {
                    ServiceURL = "http://localhost:8000/"
                };
                // Client ID is set in DynamoDB-local shell, http://localhost:8000/shell
                _client = new AmazonDynamoDBClient("TomorrowDiesToday", "fakeSecretKey", config);
            }
            else
            {
                var credentials = new TDTCredentials();
                _client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);
            }
        }

        public async Task DeleteTable()
        {       
            var tables = (await _client.ListTablesAsync()).TableNames;
            if (tables.Contains("TestTable"))
            {
                var request = new DeleteTableRequest("TestTable");
                var response = await _client.DeleteTableAsync(request);
                Console.Write(response.HttpStatusCode.ToString());
            }
        }

        public async Task Initialize()
        {
            ConfigureClient();
            var tables = (await _client.ListTablesAsync()).TableNames;
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

                var response = await _client.CreateTableAsync(request);

                Console.WriteLine(response.HttpStatusCode.ToString());
            }
            
        }

        public async Task Send(string text, string category)
        {
            var context = new DynamoDBContext(_client);
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
            var context = new DynamoDBContext(_client);
            var search = context.QueryAsync<TestItem>(GameName);
            var results = await search.GetRemainingAsync();

            SearchResultReceived(this, results);
        }
    }
}
