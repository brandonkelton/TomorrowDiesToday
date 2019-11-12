using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Models
{
    public class TileModel : IModel
    {
        #region Properties
        public string TileId { get; set; }

        public string TileName => ((TileType) int.Parse(TileId)).ToDescription();

        public bool IsActive { get; set; }

        public bool IsDoomsday { get; set; }

        public bool IsFlipped { get; set; }

        public int AlertTokens { get; set; }

        public string ImageLocation { get; set; }

        public TileStats Stats => IsDoomsday? _missionStats: IsFlipped? _flippedMissionStats: _missionStats;

        #endregion

        private TileStats _missionStats;
        private TileStats _flippedMissionStats;

        #region Constructor(s)

        public TileModel(TileType tileType)
        {
            _missionStats = new TileStats();
            _flippedMissionStats = new TileStats();
            IsActive = false;
            IsDoomsday = false;
            IsFlipped = false;
            AlertTokens = 0;
            ImageLocation = "";
        }

        public TileModel(TileType tileType, TileStats missionStats)
        {
            _missionStats = missionStats;
            IsActive = false;
            IsDoomsday = true;
            IsFlipped = false;
            AlertTokens = 0;
            ImageLocation = "";
        }

        public TileModel(TileType tileType, TileStats missionStats, TileStats flippedMissionStats)
        {
            _missionStats = missionStats;
            _flippedMissionStats = flippedMissionStats;
            IsActive = false;
            IsDoomsday = false;
            IsFlipped = false;
            AlertTokens = 0;
            ImageLocation = "";
        }

        

        #endregion
    }
}
