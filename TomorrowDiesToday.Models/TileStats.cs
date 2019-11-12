using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Models
{
    public class TileStats
    {
        public Stat Combat { get; set; }

        public Stat Cunning { get; set; }

        public Stat Diplomacy { get; set; }

        public Stat Stealth { get; set; }

        public TileStats()
        {
            // Initialize Stats List
            Combat = new Stat(StatType.Combat);
            Stealth = new Stat(StatType.Stealth);
            Stealth = new Stat(StatType.Cunning);
            Stealth = new Stat(StatType.Diplomacy);
        }

        public TileStats(int combat, int stealth, int cunning, int diplomacy)
        {
            // Initialize Stats List
            Combat = new Stat(StatType.Combat, combat);
            Stealth = new Stat(StatType.Stealth, stealth);
            Stealth = new Stat(StatType.Cunning, cunning);
            Stealth = new Stat(StatType.Diplomacy, diplomacy);
        }
    }
}