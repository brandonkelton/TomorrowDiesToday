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

        public void Send(PlayerModel model)
        {
            throw new NotImplementedException();
        }
        
        public IObservable<PlayerModel> Update => _update;

        public void RequestUpdate(PlayerRequest request)
        {
            
        }
    }
}
