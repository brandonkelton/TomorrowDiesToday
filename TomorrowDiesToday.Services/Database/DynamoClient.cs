using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TomorrowDiesToday.Services.Database.DTOs;

namespace TomorrowDiesToday.Services.Database
{
    public class DynamoClient : IDBClient, IDisposable
    {
        private IAmazonDynamoDB _client;
        private IDynamoDBContext _context;

        public DynamoClient(IAmazonDynamoDB client, IDynamoDBContext context)
        {
            _client = client;
            _context = context;
        }

        public async Task<bool> GameExists(string gameId)
        {
            var existingGame = await _context.LoadAsync<GameDTO>(gameId);
            return existingGame != null;
        }

        public async Task<bool> PlayerExists(string gameId, string playerId)
        {
            var existingPlayer = await _context.LoadAsync<PlayerDTO>(gameId, playerId);
            return existingPlayer != null;
        }

        public async Task CreateGame(string gameId)
        {
            var game = new GameDTO
            {
                GameId = gameId
            };
            await _context.SaveAsync(game);
        }

        public async Task CreatePlayer(string gameId, string playerId)
        {
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

        public async Task InitializeGameTable()
        {
            var tables = (await _client.ListTablesAsync()).TableNames;
            if (!tables.Contains("Games"))
            {
                var request = new CreateTableRequest
                {
                    TableName = "Games",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "GameId",
                            AttributeType = "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "GameId",
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

        public async Task InitializePlayerTable()
        {
            var tables = (await _client.ListTablesAsync()).TableNames;
            if (!tables.Contains("PlayerData"))
            {
                var request = new CreateTableRequest
                {
                    TableName = "PlayerData",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "GameId",
                            AttributeType = "S"
                        },
                        new AttributeDefinition
                        {
                            AttributeName = "PlayerId",
                            AttributeType = "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "GameId",
                            KeyType = "HASH"
                        },
                        new KeySchemaElement
                        {
                            AttributeName = "PlayerId",
                            KeyType = "RANGE"
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

        public async Task Update(PlayerDTO player)
        {
            await _context.SaveAsync(player);
        }

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
