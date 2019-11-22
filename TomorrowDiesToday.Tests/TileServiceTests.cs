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
        public async Task RequestActiveTilesUpdate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task SendActiveTiles()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void ToggleActive()
        {
            var activeTile = new TileModel(TileType.ArmsDealing) { IsActive = true };
            var inactiveTile = new TileModel(TileType.HackerCell) { IsActive = false };
            var hqTile = new TileModel(TileType.CIABuilding);

            var game = new GameModel
            {
                Tiles = new List<TileModel>
                {
                    activeTile,
                    inactiveTile,
                    hqTile
                }
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();

            tileService.ToggleActive(activeTile);
            tileService.ToggleActive(inactiveTile);
            tileService.ToggleActive(hqTile);

            Assert.True(!activeTile.IsActive && inactiveTile.IsActive && hqTile.IsActive);
        }

        [Fact]
        public void ToggleAgent()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void ToggleFlipped()
        {
            var flippedResourceTile = new TileModel(TileType.ArmsDealing, new TileStats(3, 1, 1, 1), new TileStats(1, 3, 1, 0))
            { 
                IsFlipped = true 
            };
            var unFlippedResourceTile = new TileModel(TileType.HackerCell, new TileStats(0, 3, 3, 0), new TileStats(0, 3, 4, 0))
            { 
                IsFlipped = false
            };
            var hqTile = new TileModel(TileType.CIABuilding, new TileStats(2, 2, 2, 2));
            var doomsdayTile = new TileModel(TileType.CrashWallStreet, new TileStats(0, 6, 8, 1));

            var game = new GameModel
            {
                Tiles = new List<TileModel>
                {
                    flippedResourceTile,
                    unFlippedResourceTile,
                    hqTile,
                    doomsdayTile
                }
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            _mockSquadService.Setup(c => c.SelectedSquadStatsUpdate).Returns(SelectedSquadStatsUpdate);
            _mockGameDataService.Setup(c => c.DataReceived).Returns(dataReceived);
            var tileService = Container.Resolve<ITileService>();

            tileService.ToggleFlipped(flippedResourceTile);
            tileService.ToggleFlipped(unFlippedResourceTile);
            tileService.ToggleFlipped(hqTile);
            tileService.ToggleFlipped(doomsdayTile);

            Assert.True(!flippedResourceTile.IsFlipped && unFlippedResourceTile.IsFlipped && !hqTile.IsFlipped && !doomsdayTile.IsFlipped);
        }

        [Fact]
        public void ToggleGlobalSecurityEvent()
        {
            throw new NotImplementedException();
        }
    }
}
