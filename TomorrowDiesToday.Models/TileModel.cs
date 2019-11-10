using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Models
{
    public class TileModel : IModel
    {
        #region Class Properties
        public TypeType TileId { get; set; }
        public bool IsFlipped { get; set; }
        public int AlertTokens { get; set; }
        public string ImageLocation { get; set; }
        public Dictionary<string, int> Stats { get; set; }
        #endregion

        private TileStats _missionStats;
        private TileStats _flippedStats;

        #region Constructor(s)
        public TileModel(TypeType tileType, TileStats missionStats, TileStats flippedMissionStats)
        {
            _missionStats = missionStats;
            _flippedStats = flippedMissionStats;
            IsFlipped = false;
            AlertTokens = 0;
            ImageLocation = "";
        }
        #endregion

        public int Combat => IsFlipped ? _flippedStats.Combat : _missionStats.Combat;

        public int Diplomacy => IsFlipped ? flippedDiplomacyValue : diplomacyValue;
    }
}
