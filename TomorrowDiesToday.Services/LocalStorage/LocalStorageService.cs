using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Services.Game;

namespace TomorrowDiesToday.Services.LocalStorage
{
    public class LocalStorageService
    {
        private GameService _gameService;

        public LocalStorageService(GameService gameService)
        {
            _gameService = gameService;
        }

        public void StoreGame()
        {
            _gameService.
            CrossSettings.Current.AddOrUpdateValue("GameState", "test");
        }
    }
}
