namespace TomorrowDiesToday.Services.LocalStorage
{
    public interface ILocalStorageService
    {
        bool GameStateExists { get; }

        void LoadGame();
        void StoreGame();
    }
}