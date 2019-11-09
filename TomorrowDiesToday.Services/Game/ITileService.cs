using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface ITileService
    {
        IObservable<Dictionary<string, TileModel>> ActiveTilesUpdate { get; }

        IObservable<Dictionary<string, TileModel>> AllTilesUpdate { get; }

        IObservable<string> ErrorMessage { get; }

        Task SendActiveTiles();

        Task RequestActiveTilesUpdate();

        void ToggleTile(TileModel tileModel);
    }
}
