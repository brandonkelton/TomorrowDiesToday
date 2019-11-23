using Autofac;
using Moq;
using TomorrowDiesToday.Services.Game;

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TomorrowDiesToday.Models;
using System.Threading.Tasks;
using TomorrowDiesToday.Models.Enums;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using System.Reactive.Subjects;
using System.Linq;

namespace TomorrowDiesToday.Tests
{
    public class TileServiceTests
    {
        public static IContainer Container;

        private Mock<IGameService> _mockGameService = new Mock<IGameService>();
        private Mock<IDataService<GameModel, GameRequest>> _mockGameDataService = new Mock<IDataService<GameModel, GameRequest>>();
        private Mock<ISquadService> _mockSquadService = new Mock<ISquadService>();

        public IObservable<GameModel> dataReceived => _update;
        private IObservable<SquadStats> SelectedSquadStatsUpdate => _selectedSquadStatsUpdate;
        private readonly ReplaySubject<SquadStats> _selectedSquadStatsUpdate = new ReplaySubject<SquadStats>(1);
        private readonly ReplaySubject<GameModel> _update = new ReplaySubject<GameModel>(1);


        public TileServiceTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TileService>().As<ITileService>().InstancePerLifetimeScope();
            builder.RegisterInstance(_mockGameService.Object).As<IGameService>().SingleInstance();
            builder.RegisterInstance(_mockGameDataService.Object).As<IDataService<GameModel, GameRequest>>().SingleInstance();
            builder.RegisterInstance(_mockSquadService.Object).As<ISquadService>().SingleInstance();
            Container = builder.Build();
        }

        [Fact]
        public void DecrementAlertTokens()
        {
            var game = new GameModel
            {
                Tiles = new List<TileModel>()
            };

            var multipleAlertTokensTile = new TileModel(TileType.GamblingDens)
            {
                AlertTokens = 2
            };
            var noAllertTokensTile = new TileModel(TileType.HackerCell)
            {
                AlertTokens = 0
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();
            tileService.DecrementAlertTokens(multipleAlertTokensTile);
            tileService.DecrementAlertTokens(noAllertTokensTile);

            Assert.True(multipleAlertTokensTile.AlertTokens == 1 && noAllertTokensTile.AlertTokens == 0);
        }

        [Fact]
        public void IncrementAlertTokens()
        {
            var game = new GameModel
            {
                Tiles = new List<TileModel>()
            };

            var multipleAlertTokensTile = new TileModel(TileType.GamblingDens)
            {
                AlertTokens = 2
            };
            var noAllertTokensTile = new TileModel(TileType.HackerCell)
            {
                AlertTokens = 0
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();
            tileService.IncrementAlertTokens(multipleAlertTokensTile);
            tileService.IncrementAlertTokens(noAllertTokensTile);

            Assert.True(multipleAlertTokensTile.AlertTokens == 3 && noAllertTokensTile.AlertTokens == 1);
        }

        [Fact]
        public async Task RequestTilesUpdate()
        {
            var game = new GameModel
            {
                GameId = "ABCDEF"
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();
            await tileService.RequestTilesUpdate();

            Assert.True(true);
        }

        [Fact]
        public async Task SendTiles()
        {
            var game = new GameModel
            {
                GameId = "ABCDEF",
                Tiles = new List<TileModel>()
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();
            await tileService.SendTiles();

            Assert.True(true);
        }

        [Fact]
        public void ToggleActive()
        {
            var game = new GameModel
            {
                Tiles = new List<TileModel>()
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();

            var activeResourceTile = game.Tiles.Where(t => t.TileType == TileType.ArmsDealing).FirstOrDefault();
            activeResourceTile.IsActive = true;
            var inactiveResourceTile = game.Tiles.Where(t => t.TileType == TileType.HackerCell).FirstOrDefault();
            var activeDoomsdayTile = game.Tiles.Where(t => t.TileType == TileType.CrashWallStreet).FirstOrDefault();
            activeDoomsdayTile.IsActive = true;
            var inactiveDoomsdayTile = game.Tiles.Where(t => t.TileType == TileType.BringDownSatellites).FirstOrDefault();
            var hqTile = game.Tiles.Where(t => t.TileType == TileType.InterpolHQ).FirstOrDefault();

            tileService.ToggleActive(activeResourceTile);
            tileService.ToggleActive(inactiveResourceTile);
            tileService.ToggleActive(activeDoomsdayTile);
            tileService.ToggleActive(inactiveDoomsdayTile);
            tileService.ToggleActive(hqTile);

            // Resource missions - toggle IsActive bool
            var resourceTileSuccess = !activeResourceTile.IsActive && inactiveResourceTile.IsActive;
            // Doomsday missions - toggle IsActive bool
            var doomsdayTileSuccess = !activeDoomsdayTile.IsActive && inactiveDoomsdayTile.IsActive;
            // Agent HQs - can not set IsActive to false
            var hqTileSuccess = hqTile.IsActive;

            Assert.True(resourceTileSuccess && doomsdayTileSuccess && hqTileSuccess);
        }
        
        [Fact]
        public void ToggleAgent()
        {
            var game = new GameModel
            {
                Tiles = new List<TileModel>()
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();

            var doomsdayTile = game.Tiles.Where(t => t.TileType == TileType.CrashWallStreet).FirstOrDefault();
            var resourceTile = game.Tiles.Where(t => t.TileType == TileType.ArmsDealing).FirstOrDefault();

            tileService.ToggleAgent(doomsdayTile);
            tileService.ToggleAgent(resourceTile);

            Assert.True(doomsdayTile.IsAgentCIA && resourceTile.IsAgentInterpol);
        }

        [Fact]
        public void ToggleFlipped()
        {
            var game = new GameModel
            {
                Tiles = new List<TileModel>()
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();

            var flippedResourceTile = game.Tiles.Where(t => t.TileType == TileType.ArmsDealing).FirstOrDefault();
            flippedResourceTile.IsFlipped = true;
            var unflippedResourceTile = game.Tiles.Where(t => t.TileType == TileType.HackerCell).FirstOrDefault();
            var doomsdayTile = game.Tiles.Where(t => t.TileType == TileType.CrashWallStreet).FirstOrDefault();
            var hqTile = game.Tiles.Where(t => t.TileType == TileType.InterpolHQ).FirstOrDefault();
            
            tileService.ToggleFlipped(flippedResourceTile);
            tileService.ToggleFlipped(unflippedResourceTile);
            tileService.ToggleFlipped(hqTile);
            tileService.ToggleFlipped(doomsdayTile);

            Assert.True(!flippedResourceTile.IsFlipped && unflippedResourceTile.IsFlipped && !hqTile.IsFlipped && !doomsdayTile.IsFlipped);
        }

        [Fact]
        public void ToggleGlobalSecurityEvent()
        {
            var game = new GameModel
            {
                Tiles = new List<TileModel>()
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();

            var doomsdayTile = game.Tiles.Where(t => t.TileType == TileType.CrashWallStreet).FirstOrDefault();
            var hqTile = game.Tiles.Where(t => t.TileType == TileType.CIABuilding).FirstOrDefault();
            hqTile.IsAgentCIA = false;
            var resourceTile = game.Tiles.Where(t => t.TileType == TileType.ArmsDealing).FirstOrDefault();
            resourceTile.IsActive = true;
            
            tileService.ToggleGlobalSecurityEvent();

            // Resource missions - all Stats increase by 1
            var resourceTileSuccess = resourceTile.Stats.Combat.Value == 4
                                   && resourceTile.Stats.Stealth.Value == 2
                                   && resourceTile.Stats.Cunning.Value == 2
                                   && resourceTile.Stats.Diplomacy.Value == 2;
            // Doomsday missions - Stats do not change
            var doomsdayTileSuccess = doomsdayTile.Stats.Combat.Value == 0
                                   && doomsdayTile.Stats.Stealth.Value == 6
                                   && doomsdayTile.Stats.Cunning.Value == 8
                                   && doomsdayTile.Stats.Diplomacy.Value == 1;
            // Agent HQs - Stats do not change
            var hqTileSuccess = hqTile.Stats.Combat.Value == 2
                             && hqTile.Stats.Stealth.Value == 2
                             && hqTile.Stats.Cunning.Value == 2
                             && hqTile.Stats.Diplomacy.Value == 2;

            Assert.True(resourceTileSuccess && doomsdayTileSuccess && hqTileSuccess);
        }
    }
}
