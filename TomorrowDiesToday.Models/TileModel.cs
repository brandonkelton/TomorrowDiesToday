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

        public bool IsGlobalSecurityEvent { get; set; }

        public bool IsHQ { get; set; }

        public bool IsFlipped { get; set; }

        public int AlertTokens { get; set; }

        public string ImageLocation { get; set; }

        public TileStats Stats => IsDoomsday? 
                                    IsGlobalSecurityEvent? _missionStats.IncreaseAll(1) : _missionStats : 
                                  IsFlipped?
                                    IsGlobalSecurityEvent? _flippedMissionStats.IncreaseAll(1) : _flippedMissionStats :
                                  IsGlobalSecurityEvent? _missionStats.IncreaseAll(1) : _missionStats;

        public TileStats Stats
        {
            get
            {
                TileStats modifiedStats= new TileStats();

                if (IsFlipped) {
                    modifiedStats = _flippedMissionStats;
                }
                else {
                    modifiedStats = _missionStats;
                }

                if (IsHQ) {
                    modifiedStats.Combat.SetValue(modifiedStats.Combat.Value * AlertToken);
                    modifiedStats.Stealth.SetValue(modifiedStats.Combat.Value * AlertToken);
                    modifiedStats.Cunning.SetValue(modifiedStats.Combat.Value * AlertToken);
                    modifiedStats.Diplomacy.SetValue(modifiedStats.Combat.Value * AlertToken);
                }
                if (IsDoomsday) {
                    //Doomsdaystuff
                }
                if (IsGlobalSecurityEvent) {
                    modifiedStats = modifiedStats.IncreaseAll(1);
                }
                return modifiedStats;
            }
        }

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
            if (tileType == TileType.CIABuilding || tileType == TileType.InterpolHQ)
            {
                IsHQ = true;
                IsDoomsday = false;
            }
            else
            {
                IsDoomsday = true;
                IsHQ = false;
            }
            _missionStats = missionStats;
            IsActive = false;
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
