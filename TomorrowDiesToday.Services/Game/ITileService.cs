using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface ITileService
    {
        IObservable<List<TileModel>> TilesUpdate { get; }

        IObservable<string> ErrorMessage { get; }

        Task SendActiveTiles();

        Task RequestActiveTilesUpdate();

        void ToggleTile(TileModel tileModel);
    }
}
