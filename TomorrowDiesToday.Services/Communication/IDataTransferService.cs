using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.DB.DTOs;

namespace TomorrowDiesToday.Services.Communication
{
    public interface IDataTransferService
    {
        event EventHandler<SquadModel> SquadRequestReceived;
        event EventHandler<List<SquadModel>> SquadsRequestReceived;
        event EventHandler<SquadModel> SquadUpdateReceived;

        Task RequestSquad(SquadModel squadModel);

        Task UpdateSquad(SquadModel squadModel);
    }
}
