using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Data
{
    public class SquadDataService : IDataService<SquadModel, SquadRequest>
    {
        private ReplaySubject<SquadModel> _update = new ReplaySubject<SquadModel>(1);
        public IObservable<SquadModel> Update => _update;

        public void RequestUpdate(SquadRequest request)
        {
            throw new NotImplementedException();
        }

        public void Send(SquadModel model)
        {
            throw new NotImplementedException();
        }
    }
}
