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

        public string TileName { get; set; }

        public bool IsActive { get; set; }

        public bool IsFlipped { get; set; }

        public int AlertTokens { get; set; }

        public string ImageLocation { get; set; }

        public List<Stat> Stats { get; set; } = new List<Stat>();
        #endregion

        private TileStats _missionStats;
        private TileStats _flippedStats;

        #region Constructor(s)
        public TileModel(TileType tileType, TileStats missionStats, TileStats flippedMissionStats)
        {
            _missionStats = missionStats;
            _flippedStats = flippedMissionStats;
            IsFlipped = false;
            AlertTokens = 0;
            ImageLocation = "";
        }
        #endregion
    }
}
