using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    class Squad : ISquad
    {

        public Dictionary<string, int> SquadData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<string, int> SquadStats { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Squad()
        {
            SquadStats.Add("Combat", 0);
            SquadStats.Add("Stealth", 0);
            SquadStats.Add("Cunning", 0);
            SquadStats.Add("Diplomacy", 0);

            SquadData.Add("Thief", 0);
            SquadData.Add("Hacker", 0);
            SquadData.Add("Soldier", 0);
            SquadData.Add("Assassin", 0);
            SquadData.Add("Fixer", 0);
            SquadData.Add("Scientist", 0);

            SquadData.Add("Faced Henchmen", 0);

            SquadData.Add("Hypnotic Spray", 0);
            SquadData.Add("Explosive Rounds", 0);

            SquadData.Add("Ugo Combat", 0);
            SquadData.Add("Ugo Stealth", 0);
            SquadData.Add("Ugo Cunning", 0);
            SquadData.Add("Ugo Diplomacy", 0);
        }
    }
}
