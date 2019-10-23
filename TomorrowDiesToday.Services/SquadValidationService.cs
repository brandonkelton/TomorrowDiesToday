using System;
using System.Collections.Generic;

namespace TomorrowDiesToday.Services
{
    class SquadValidationService
    {
        public const int MAX_SQUAD_SIZE = 6;
        public const int NUMBER_OF_FACED_HENCHMAN = 9;
        public const int DATA_STRIP_LENGTH = 13; 

        public Boolean ValidateSquad(Dictionary<string, Dictionary<string, int>> squadData) {

            int unitTotal = squadData["Thief"] + squadData["Hacker"] + squadData["Soldier"] 
                + squadData["Assassin"] + squadData["Fixer"] + squadData["Scientist"];

            int ugoTotal = squadData["Ugo Combat"] + squadData["Ugo Stealth"] + squadData["Ugo Cunning"] + squadData["Ugo Diplomacy"];

            if (squadData.Count != DATA_STRIP_LENGTH) {
                return false;
            }

            if (unitTotal > MAX_SQUAD_SIZE || unitTotal < 0) {
                return false;
            }

            if (squadData["Faced Henchman"] > NUMBER_OF_FACED_HENCHMAN || squadData["Faced Henchman"] < 0) {
                return false;
            }

            if (squadData["Hypnotic Spray"] > 1 || squadData["Hypnotic Spray"] < 0) {
                return false;
            }

            if (squadData["Explosive Rounds"] > 1 || squadData["Explosive Rounds"] < 0)
            {
                return false;
            }

            if (ugoTotal > MAX_SQUAD_SIZE || ugoTotal < 0) {
                return false;
            }

            return true;

        }

    }
}
