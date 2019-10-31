using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class TileModel : IModel
    {
        #region Class Properties
        public bool isFlipped { get; set; }
        public int alertTokens { get; set; }
        public string imageLocation { get; set; }
        public Dictionary<string, int> tileStats { get; set; }
        #endregion

        #region Constructor(s)
        public Tile()
        {
            isToggled = false;
            alertTokens = 0;
            imageLocation = "";

            tileStats.Add("Combat", 0);
            tileStats.Add("Stealth", 0);
            tileStats.Add("Cunning", 0);
            tileStats.Add("Diplomacy", 0);
        }

        #endregion
    }
}
