using System.Threading.Tasks;

namespace TomorrowDiesToday.Services.LocalStorage
{
    public interface ILocalStorageService
    {
        Task DeleteGame();
        Task<bool> GetGameExists();
        Task<string> GetGameId();
        Task LoadGame();
        Task SaveGame();
    }
}