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
        public event EventHandler<List<Dictionary<string, AttributeValue>>> SquadRequestReceived;
        public event EventHandler<List<Dictionary<string, AttributeValue>>> SquadsRequestReceived;
        private AmazonDynamoDBClient _client;
        private bool _altConfig = true;

        private string PlayerName = "Test Player";


        public void ConfigureClient()
        {
            if (_altConfig)
            { // Use DynamoDB-local
                var config = new AmazonDynamoDBConfig
                {
                    // Replace localhost with server IP to connect with DynamoDB-local on remote server
                    ServiceURL = "http://localhost:8000/"
                };

                // Client ID is set in DynamoDB-local shell, http://localhost:8000/shell
                _client = new AmazonDynamoDBClient("TomorrowDiesToday", "fakeSecretKey", config);
            }
            else
            { // Use AWS DynamoDB
                var credentials = new TDTCredentials();
                _client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);
            }
        }

        public async Task DeleteTable()
        {
            var tables = (await _client.ListTablesAsync()).TableNames;
            if (tables.Contains("GameTable"))
            {
                var request = new DeleteTableRequest("GameTable");
                var response = await _client.DeleteTableAsync(request);
                Console.Write(response.HttpStatusCode.ToString());
            }
        }

        public async Task Initialize()
        {
            ConfigureClient();
            var tables = (await _client.ListTablesAsync()).TableNames;
            if (!tables.Contains("GameTable"))
            {
                var request = new CreateTableRequest
                {
                    TableName = "GameTable",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "GameId",
                            AttributeType = "S"
                        },
                        new AttributeDefinition
                        {
                            AttributeName = "SquadId",
                            AttributeType = "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "GameId",
                            KeyType = "RANGE"
                        },
                        new KeySchemaElement
                        {
                            AttributeName = "SquadId",
                            KeyType = "HASH"
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                };

                await _client.CreateTableAsync(request);
            }
        }

        public async Task RequestSquads(string gameId)
        {
            var request = new ScanRequest
            {
                TableName = "GameTable",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                  { "#id", "GameId" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":v_GameId", new AttributeValue { S = gameId } }
                },
                FilterExpression = "#id = :v_GameId",
                ProjectionExpression = "#id, SquadId, PlayerId, SquadData"
            };

            var results = await _client.ScanAsync(request);
            SquadsRequestReceived(this, results.Items);
        }

        public async Task RequestSquad(SquadRequestDTO squadDTO)
        {
            var request = new QueryRequest
            {
                TableName = "GameTable",
                KeyConditionExpression = "SquadId = :v_SquadId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_SquadId", new AttributeValue { S =  squadDTO.SquadId }}}
            };
            var results = await _client.QueryAsync(request);

            SquadRequestReceived(this, results.Items);
        }

        public async Task SendSquad(SquadUpdateDTO squadDTO)
        {
            var context = new DynamoDBContext(_client);
            await context.SaveAsync(squadDTO);
        }
    }
}
