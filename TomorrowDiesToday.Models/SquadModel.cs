using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class SquadModel : IModel
    {
        public string PlayerId { get; set; }

        public string SquadId { get; set; }

        public Dictionary<string, int> Data { get; set; }
        public Dictionary<string, int> Stats { get; set; }

        public bool IsSelected { get; set; }

        public SquadModel()
        {
            //// Initialize Data Dictionary

            Data.Add("Named Henchman", 0);
            // Named Henchmen =>
            // 1 = Archibald Kluge
            // 2 = Axle Robbins
            // 3 = Azura Badeau
            // 4 = Boris "Myasneek"
            // 5 = Cassandra O'Shea
            // 6 = Emmerson Barlow
            // 7 = Jin Feng
            // 8 = The Node
            // 9 = Ugo Dottore

            Data.Add("Thief", 0);
            Data.Add("Hacker", 0);
            Data.Add("Soldier", 0);
            Data.Add("Assassin", 0);
            Data.Add("Fixer", 0);
            Data.Add("Scientist", 0);
            
            Data.Add("Hypnotic Spray", 0);
            Data.Add("Explosive Rounds", 0);

            Data.Add("Ugo Combat", 0);
            Data.Add("Ugo Stealth", 0);
            Data.Add("Ugo Cunning", 0);
            Data.Add("Ugo Diplomacy", 0);

            //// Initialize Stats Dictionary
            Stats.Add("Combat", 0);
            Stats.Add("Stealth", 0);
            Stats.Add("Cunning", 0);
            Stats.Add("Diplomacy", 0);

            //// Initialize as not selected
            IsSelected = false;
        }
    }
}
