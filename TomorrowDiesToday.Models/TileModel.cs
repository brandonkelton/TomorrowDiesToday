using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class TileModel : IModel
    {
        #region Class Properties
        public string TileId { get; set; }
        public string TileName { get; set; }
        public bool IsFlipped { get; set; }
        public int AlertTokens { get; set; }
        public string ImageLocation { get; set; }
        public Dictionary<string, int> Stats { get; set; }
        #endregion

        #region Constructor(s)
        public TileModel()
        {
            IsFlipped = false;
            AlertTokens = 0;
            ImageLocation = "";

            Stats.Add("Combat", 0);
            Stats.Add("Stealth", 0);
            Stats.Add("Cunning", 0);
            Stats.Add("Diplomacy", 0);
        }
        #endregion
    }
}
