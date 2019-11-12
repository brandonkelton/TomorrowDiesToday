using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Models
{
    public class ArmamentStats
    {
        public Stat Combat { get; set; }

        public Stat Cunning { get; set; }

        public Stat Diplomacy { get; set; }

        public Stat Stealth { get; set; }

        public ArmamentStats(int combat, int stealth, int cunning, int diplomacy)
        {
            // Initialize Stats List
            Combat = new Stat(StatType.Combat, combat);
            Cunning = new Stat(StatType.Cunning, cunning);
            Diplomacy = new Stat(StatType.Diplomacy, diplomacy);
            Stealth = new Stat(StatType.Stealth, stealth);
        }
    }
}
