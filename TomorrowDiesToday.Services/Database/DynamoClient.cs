using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TomorrowDiesToday.Services.Database.DTOs;

namespace TomorrowDiesToday.Services.Database
{
    public class DynamoClient : IDBClient, IDisposable
    {
        private AmazonDynamoDBClient _client;
        private DynamoDBContext _context;
        private bool _altConfig = true;

        public DynamoClient()
        {
            Initialize();
        }

        private void Initialize()
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

            _context = new DynamoDBContext(_client);

        }

        public async Task CreateGame(string gameId)
        {
            var existingGame = await _context.LoadAsync<GameDTO>(gameId);
            if (existingGame != null)
            {
                // TODO: create app specific exception and handle at a higher level
                // ex: GameExistsException(gameId)
                throw new Exception("Game already exists!");
            }

            var game = new GameDTO
            {
                GameId = gameId,
                Created = DateTime.UtcNow
            };
            await _context.SaveAsync(game);
        }

        public async Task CreatePlayer(string gameId, string playerId)
        {
            var existingPlayer = await _context.LoadAsync<PlayerDTO>(gameId, playerId);
            if (existingPlayer != null)
            {
                // TODO: create app specific exception and handle at a higher level
                // ex:  PlayerExistsException(playerId)
                throw new Exception("Player already exists!");
            }

            var player = new PlayerDTO
            {
                GameId = gameId,
                PlayerId = playerId,
                Squads = new List<SquadDTO>()
            };
            await _context.SaveAsync(player);
        }

        public async Task DeleteGame(string gameId, string playerId)
        {
            // NOTE: possible better solutions, but delete player when the player exits game,
            // if no more players exist, then delete game; checking on game load for
            // stale games would require a full table load, which would kill our
            // buffer on free data.

            await _context.DeleteAsync<PlayerDTO>(gameId, playerId);

            if ((await RequestPlayerList(gameId)).Count == 0)
            {
                await _context.DeleteAsync<GameDTO>(gameId);
            }
        }

        public async Task<List<PlayerDTO>> RequestPlayerList(string gameId)
        {
            var search = _context.QueryAsync<PlayerDTO>(gameId);
            var results = await search.GetRemainingAsync();
            return results;
        }

        public async Task<PlayerDTO> RequestPlayer(string gameId, string playerId)
        {
            var player = await _context.LoadAsync<PlayerDTO>(gameId, playerId);
            return player;
        }

        //public async Task Initialize()
        //{
        //    ConfigureClient();
        //    var tables = (await _client.ListTablesAsync()).TableNames;
        //    if (!tables.Contains("GameTable"))
        //    {
        //        var request = new CreateTableRequest
        //        {
        //            TableName = "GameTable",
        //            AttributeDefinitions = new List<AttributeDefinition>
        //            {
        //                new AttributeDefinition
        //                {
        //                    AttributeName = "GameId",
        //                    AttributeType = "S"
        //                },
        //                new AttributeDefinition
        //                {
        //                    AttributeName = "SquadId",
        //                    AttributeType = "S"
        //                }
        //            },
        //            KeySchema = new List<KeySchemaElement>
        //            {
        //                new KeySchemaElement
        //                {
        //                    AttributeName = "GameId",
        //                    KeyType = "RANGE"
        //                },
        //                new KeySchemaElement
        //                {
        //                    AttributeName = "SquadId",
        //                    KeyType = "HASH"
        //                }
        //            },
        //            ProvisionedThroughput = new ProvisionedThroughput
        //            {
        //                ReadCapacityUnits = 5,
        //                WriteCapacityUnits = 5
        //            }
        //        };

        //        await _client.CreateTableAsync(request);
        //    }
        //}

        //public async Task DeleteTable()
        //{
        //    var tables = (await _client.ListTablesAsync()).TableNames;
        //    if (tables.Contains("GameTable"))
        //    {
        //        var request = new DeleteTableRequest("GameTable");
        //        var response = await _client.DeleteTableAsync(request);
        //        Console.Write(response.HttpStatusCode.ToString());
        ////    }
        ////}

        public void Dispose()
        {
            _context.Dispose();
            _client.Dispose();
        }
    }
}
