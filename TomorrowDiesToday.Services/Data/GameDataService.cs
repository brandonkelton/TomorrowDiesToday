using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Data
{
    public class GameDataService : IDataService<GameModel, GameRequest>
    {
        private readonly ReplaySubject<GameModel> _update = new ReplaySubject<GameModel>(1);

        public void Send(GameModel model)
        {
            throw new NotImplementedException();
        }

        public IObservable<GameModel> Update => _update;

        public void RequestUpdate(GameRequest request)
        {
            
        }
    }
}
