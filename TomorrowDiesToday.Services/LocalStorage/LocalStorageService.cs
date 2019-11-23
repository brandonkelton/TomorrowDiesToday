using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Models.Templates;
using TomorrowDiesToday.Services.Game;

namespace TomorrowDiesToday.Services.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        private IGameService _gameService;
        const string GAME_STATE_FILENAME = "GameState.TomorrowNeverDies";

        public LocalStorageService(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<bool> GetGameExists()
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult fileExists = await folder.CheckExistsAsync(GAME_STATE_FILENAME);
            if (fileExists == ExistenceCheckResult.FileExists)
            {
                IFile file = await folder.GetFileAsync(GAME_STATE_FILENAME);
                var serializedGame = await file.ReadAllTextAsync();
                if (serializedGame == null || serializedGame == "null")
                {
                    await DeleteGame();
                    return false;
                }
                return true;
            }
            return false;
        }

        public async Task<string> GetGameId()
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult fileExists = await folder.CheckExistsAsync(GAME_STATE_FILENAME);
            if (fileExists == ExistenceCheckResult.FileExists)
            {
                IFile file = await folder.GetFileAsync(GAME_STATE_FILENAME);
                var serializedGame = await file.ReadAllTextAsync();
                if (serializedGame != null)
                {
                    var gameState = JsonConvert.DeserializeObject<GameModel>(serializedGame);
                    if (gameState != null)
                    {
                        return gameState.GameId;
                    }
                }
            }

            return null;
        }

        public async Task SaveGame()
        {
            if (_gameService.Game == null)
            {
                return;
            }

            IFolder folder = FileSystem.Current.LocalStorage;
            IFile file = await folder.CreateFileAsync(GAME_STATE_FILENAME, CreationCollisionOption.ReplaceExisting);
            var serializedGame = JsonConvert.SerializeObject(_gameService.Game);
            await file.WriteAllTextAsync(serializedGame);
        }

        public async Task LoadGame()
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult gameExists = await folder.CheckExistsAsync(GAME_STATE_FILENAME);
            if (gameExists == ExistenceCheckResult.FileExists)
            {
                IFile file = await folder.GetFileAsync(GAME_STATE_FILENAME);
                var serializedGame = await file.ReadAllTextAsync();
                if (serializedGame != null)
                {
                    var gameState = JsonConvert.DeserializeObject<GameModel>(serializedGame);
                    _gameService.SetGame(gameState);
                }
            }
        }

        public async Task DeleteGame()
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult gameExists = await folder.CheckExistsAsync(GAME_STATE_FILENAME);
            if (gameExists == ExistenceCheckResult.FileExists)
            {
                IFile file = await folder.GetFileAsync(GAME_STATE_FILENAME);
                await file.DeleteAsync();
            }
        }
    }
}
