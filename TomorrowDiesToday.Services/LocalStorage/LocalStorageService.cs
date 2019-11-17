using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Game;

namespace TomorrowDiesToday.Services.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        private GameService _gameService;
        const string GAME_STATE_KEY = "GameState";

        public LocalStorageService(GameService gameService)
        {
            _gameService = gameService;
        }

        public bool GameStateExists => CrossSettings.Current.Contains(GAME_STATE_KEY);

        public void StoreGame()
        {
            var serializedGameState = JsonConvert.SerializeObject(_gameService.Game);
            CrossSettings.Current.AddOrUpdateValue(GAME_STATE_KEY, serializedGameState);
        }

        public void LoadGame()
        {
            var serializedGameState = CrossSettings.Current.GetValueOrDefault(GAME_STATE_KEY, null);
            if (serializedGameState != null)
            {
                var gameState = JsonConvert.DeserializeObject<GameModel>(serializedGameState);
                _gameService.SetGame(gameState);
            }
        }
    }
}
