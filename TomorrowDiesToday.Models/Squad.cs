using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    class Squad : ISquad
    {

        public Dictionary<string, int> SquadData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<string, int> SquadStats { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void InitializeSquadStats()
        {
            SquadStats.Add("Combat", 0);
            SquadStats.Add("Stealth", 0);
            SquadStats.Add("Cunning", 0);
            SquadStats.Add("Diplomacy", 0);
        }
    }
}
