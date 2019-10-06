using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Data
{
    public class GameDataService : IDataService<GameModel, GameRequest>
    {
        private readonly ReplaySubject<GameModel> _update = new ReplaySubject<GameModel>(1);

        public IObservable<GameModel> DataReceived => _update;

        public async Task Create(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exists(string id)
        {
            throw new NotImplementedException();
        }

        public async Task RequestUpdate(GameRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task Update(GameModel model)
        {
            throw new NotImplementedException();
        }
    }
}
