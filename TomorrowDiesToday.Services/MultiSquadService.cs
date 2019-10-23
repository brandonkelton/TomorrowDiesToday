using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services
{
    class MultiSquadService
    {
        public Dictionary<string, int> AddSquadStats(params Dictionary<string, int>[] squads)
        {
            int combatTotal = 0;
            int stealthTotal = 0;
            int cunningTotal = 0;
            int diplomacyTotal = 0;

            Dictionary<string, int> squadTotals = new Dictionary<string, int>();

            foreach (Dictionary<string, int> squad in squads)
            {
                combatTotal += squad["Combat"];
                stealthTotal += squad["Stealth"];
                cunningTotal += squad["Cunning"];
                diplomacyTotal += squad["Diplomacy"];
            }

            squadTotals.Add("Combat", combatTotal);
            squadTotals.Add("Stealth", stealthTotal);
            squadTotals.Add("Cunning", cunningTotal);
            squadTotals.Add("Diplomacy", diplomacyTotal);

            return squadTotals;
        }

    }
}
