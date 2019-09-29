﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.DB;
using TomorrowDiesToday.DB.DTOs;
using Amazon.DynamoDBv2.Model;

namespace TomorrowDiesToday.Services.Communication
{
    /// <summary>
    /// This service will receive models and update-requests from the DataService, and then send the corresponding DTO
    /// to the DynamoClient.  For update requests the service waits for DynamoClient to return a DTO and then returns
    /// the corresponding model to the DataService.
    /// </summary>
    public class DataTransferService : IDataTransferService
    {
        public event EventHandler<SquadModel> SquadRequestReceived;
        public event EventHandler<List<SquadModel>> SquadsRequestReceived;
        public event EventHandler<SquadModel> SquadUpdateReceived;
        private DynamoClient _client = new DynamoClient();


        public async Task Initialize()
        {
            await _client.Initialize();
            _client.SquadRequestReceived += OnSquadRequestReceived;
            _client.SquadsRequestReceived += OnSquadsRequestReceived;
        }

        public async Task RequestSquad(SquadModel squadModel)
        {
            var squadDTO = new SquadRequestDTO
            {
                SquadId = squadModel.SquadId,
                GameId = squadModel.GameId
            };

            await _client.RequestSquad(squadDTO);
        }

        public async Task RequestOtherSquads(GameModel game)
        {
            await _client.RequestOtherSquads(game.GameId, game.MyPlayerId);
        }

        public async Task UpdateSquad(SquadModel squadModel)
        {
            var squadDTO = new SquadUpdateDTO
            {
                SquadId = squadModel.SquadId,
                GameId = squadModel.GameId,
                PlayerId = squadModel.PlayerId,
                SquadData = string.Join(",", squadModel.Data)
            };

            await _client.SendSquad(squadDTO);
        }

        private List<SquadModel> ConvertSquadResponse(List<Dictionary<string, AttributeValue>> squads)
        {
            var squadModels = new List<SquadModel>();

            for (int i = 0; i < squads.Count; i++)
            {
                var squadModel = new SquadModel();

                foreach (KeyValuePair<string, AttributeValue> entry in squads[i])
                {
                    if (entry.Key == "SquadId")
                    {
                        squadModel.SquadId = entry.Value.S;
                    }
                    else if (entry.Key == "GameId")
                    {
                        squadModel.GameId = entry.Value.S;
                    }
                    else if (entry.Key == "PlayerId")
                    {
                        squadModel.PlayerId = entry.Value.S;
                    }
                    else if (entry.Key == "PlayerData")
                    {
                        squadModel.Data = Array.ConvertAll(entry.Value.S.Split(','), s => int.Parse(s));
                    }
                }
                squadModels.Add(squadModel);
            }
            return squadModels;
        }

        private void OnSquadRequestReceived(object sender, List<Dictionary<string, AttributeValue>> squads)
        {
            SquadRequestReceived(this, ConvertSquadResponse(squads)[0]);
        }

        private void OnSquadsRequestReceived(object sender, List<Dictionary<string, AttributeValue>> squads)
        {
            SquadsRequestReceived(this, ConvertSquadResponse(squads));
        }
    }
}
