using System.Threading.Tasks;

namespace TomorrowDiesToday.Services.LocalStorage
{
    public interface ILocalStorageService
    {
        Task DeleteGame();
        bool GameExists { get; }
        Task<string> GetGameId();
        Task LoadGame();
        Task StoreGame();
    }
}