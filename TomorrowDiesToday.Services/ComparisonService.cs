using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services
{
    class ComparisonService
    {
        public bool SuccessCheck(Dictionary<string, int> tileStats, Dictionary<string, int> squadStats)
        {
            int combatResult = squadStats["Combat"] - tileStats["Combat"];
            int stealthResult = squadStats["Stealth"] - tileStats["Stealth"];
            int cunningResult = squadStats["Cunning"] - tileStats["Cunning"];
            int diplomacyResult = squadStats["Diplomacy"] - tileStats["Diplomacy"];

            if(combatResult >= 0 && stealthResult >= 0 && cunningResult >= 0 && diplomacyResult >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
