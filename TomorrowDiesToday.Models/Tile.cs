using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    class Tile : ITile
    {
        #region Class Properties
        public bool isFlipped { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int alertTokens { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string imageLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<string, int> tileStats { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
