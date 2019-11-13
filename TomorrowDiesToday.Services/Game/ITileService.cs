using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Game
{
    public interface ITileService
    {
        IObservable<List<TileModel>> ActiveTilesUpdate { get; }

        IObservable<List<TileModel>> AllTilesUpdate { get; }

        IObservable<string> ErrorMessage { get; }

        Task SendActiveTiles();

        Task RequestActiveTilesUpdate();

        void ToggleActive(TileModel tileModel);

        void ToggleFlipped(TileModel tileModel);
    }
}
