using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Data
{
    public class PlayerDataService : IDataService<PlayerModel, PlayerRequest>
    {
        private readonly ReplaySubject<PlayerModel> _update = new ReplaySubject<PlayerModel>(1);

        public IObservable<PlayerModel> DataReceived => _update;

        public async Task Create(PlayerModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exists(PlayerModel model)
        {
            throw new NotImplementedException();
        }

        public async Task RequestUpdate(PlayerRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task Update(PlayerModel model)
        {
            throw new NotImplementedException();
        }
    }
}
