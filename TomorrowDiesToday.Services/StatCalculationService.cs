
using System;
using System.Collections.Generic;

namespace TomorrowDiesToday.Services
{
    class StatCalculationService
    {
        public Dictionary<string, int> ConvertToDictionary(string dataStrip)
        {
            string[] keyArray = new string[] {"Thief", "Hacker", "Soldier", "Assassin", "Fixer", "Scientist",
                "Faced Henchmen", "Hypnotic Spray", "Explosive Rounds", "Ugo Combat", "Ugo Stealth", "Ugo Cunning", "Ugo Diplomacy" };
            string[] stringArray = dataStrip.Split(',');
            int[] valueArray = Array.ConvertAll(stringArray, int.Parse);
            Dictionary<string, int> squadData = new Dictionary<string, int>();

            
            if(keyArray.Length == valueArray.Length)
            {
                for(int i = 0; i < keyArray.Length; i++)
                {
                    squadData.Add(keyArray[i], valueArray[i]);
                }
            }

            return squadData;
        }

    }
}
