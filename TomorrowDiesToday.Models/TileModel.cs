using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Models
{
    public class TileModel : IModel
    {
        #region Properties

        public int AlertTokens { get; set; }

        public string ImageLocation { get; set; }

        public bool IsActive { get; set; }

        public bool IsAgentCIA { get; set; }

        public bool IsAgentInterpol { get; set; }

        public bool IsDoomsday { get; set; }

        public bool IsFlipped { get; set; }

        public bool IsGlobalSecurityEvent { get; set; }

        public bool IsHQ { get; set; }

        public bool IsHacked { get; set; }

        public TileStats Stats
        {
            get
            {
                TileStats stats = new TileStats();

                if (IsFlipped)
                {
                    stats.Combat.SetValue(_flippedMissionStats.Combat.Value);
                    stats.Stealth.SetValue(_flippedMissionStats.Stealth.Value);
                    stats.Cunning.SetValue(_flippedMissionStats.Cunning.Value);
                    stats.Diplomacy.SetValue(_flippedMissionStats.Diplomacy.Value);
                }
                else 
                {
                    stats.Combat.SetValue(_missionStats.Combat.Value);
                    stats.Stealth.SetValue(_missionStats.Stealth.Value);
                    stats.Cunning.SetValue(_missionStats.Cunning.Value);
                    stats.Diplomacy.SetValue(_missionStats.Diplomacy.Value);
                }

                if (IsHacked)
                {
                    stats.Cunning.SetValue(stats.Cunning.Value + 2);
                }

                if (IsAgentCIA)
                {
                    stats = stats.IncreaseAll(2);
                }

                if (IsAgentInterpol)
                {
                    stats.Combat.SetValue(stats.Combat.Value + 1);
                    stats.Stealth.SetValue(stats.Stealth.Value + 2);
                    stats.Cunning.SetValue(stats.Cunning.Value + 2);
                    stats.Diplomacy.SetValue(stats.Diplomacy.Value + 1);
                }

                if (IsHQ)
                {
                    if (AlertTokens > 0)
                    {
                        stats = stats.MultiplyAll(AlertTokens);
                    }
                }

                if (IsDoomsday) 
                {
                    // Doomsday stuff
                }

                if (IsGlobalSecurityEvent && !IsHQ  && !IsDoomsday) 
                {
                    stats = stats.IncreaseAll(1);
                }

                return stats;
            }
        }

        public bool Success { get; set; }

        public string TileId { get; set; }

        public string TileName => TileType.ToDescription();

        public TileType TileType { get; set; }

        #endregion

        private TileStats _missionStats;
        private TileStats _flippedMissionStats;

        #region Constructor(s)

        public TileModel(TileType tileType)
        {
            if (tileType == TileType.CIABuilding)
            {
                IsAgentCIA = true;
                IsAgentInterpol = false;
                IsHQ = true;
                IsDoomsday = false;
                IsActive = true;
            }
            else if (tileType == TileType.InterpolHQ)
            {
                IsAgentCIA = false;
                IsAgentInterpol = true;
                IsHQ = true;
                IsDoomsday = false;
                IsActive = true;
            }
            else
            {
                IsAgentCIA = false;
                IsAgentInterpol = false;
                IsDoomsday = true;
                IsHQ = false;
                IsActive = false;
            }
            _missionStats = new TileStats();
            _flippedMissionStats = new TileStats();
            IsFlipped = false;
            AlertTokens = 0;
            ImageLocation = "";
            TileType = tileType;
        }

        public TileModel(TileType tileType, TileStats missionStats)
        {
            if (tileType == TileType.CIABuilding)
            {
                IsAgentCIA = true;
                IsAgentInterpol = false;
                IsHQ = true;
                IsDoomsday = false;
                IsActive = true;
            }
            else if(tileType == TileType.InterpolHQ)
            {
                IsAgentCIA = false;
                IsAgentInterpol = true;
                IsHQ = true;
                IsDoomsday = false;
                IsActive = true;
            }
            else
            {
                IsAgentCIA = false;
                IsAgentInterpol = false;
                IsDoomsday = true;
                IsHQ = false;
                IsActive = false;
            }
            _missionStats = missionStats;
            IsFlipped = false;
            AlertTokens = 0;
            ImageLocation = "";
            TileType = tileType;
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
            TileType = tileType;
        }

        #endregion
    }
}
