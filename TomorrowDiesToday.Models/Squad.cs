using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class Squad
    {
        public Dictionary<string, int> squadStats { get; set; }
        public Dictionary<string, int> squadData { get; set; }

        public string Id { get; set; }

        #region Constructor(s)
        public Squad()
        {
            //Initialize Squad Data Dictionary
            squadData.Add("Soldier", 0);
            squadData.Add("Assassin", 0);
            squadData.Add("Thief", 0);
            squadData.Add("Scientist", 0);
            squadData.Add("Fixer", 0);
            squadData.Add("Hacker", 0);
            squadData.Add("Faced Henchman", 0);
            squadData.Add("Explosive Rounds", 0);
            squadData.Add("Hypnotic Spray", 0);
            squadData.Add("Hugo Combat", 0);
            squadData.Add("Hugo Steath", 0);
            squadData.Add("Hugo Cunning", 0);
            squadData.Add("Hugo Diplomacy", 0);

            //Initialize Squad Stats Dictionary
            squadStats.Add("Combat", 0);
            squadStats.Add("Stealth", 0);
            squadStats.Add("Cunning", 0);
            squadStats.Add("Diplomacy", 0);
        }
        #endregion
    }
}
