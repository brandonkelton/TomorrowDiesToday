using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services
{
    class TileManagementService
    {
        private Dictionary<string, Dictionary<string, int>> missionDictionary = new Dictionary<string, string> //thisDictionary <TileName, TileStats>
            {
                //Resource Missions
                { "Blood Diamond Harvest", "3,2,1,0" },
                { "Skin Trade", "1,1,2,2" },
                { "Social Engineering Scams", "0,1,3,2" },
                { "Counterfeiting Operation", "0,3,2,1" },
                { "Ponzi Schemes", "0,1,2,3" },
                { "Political Corruption", "0,2,2,2" },
                { "Hacker Cell", "0,3,3,0" },
                { "Art Thievery", "1,2,2,1" },
                { "Exotic Car GTA", "0,2,4,0" },
                { "Smuggling Ring", "0,4,1,1" },
                { "Arms Dealing", "3,1,1,1" },
                { "Maritime Piracy", "3,1,2,0" },
                { "Rig Sports Events", "0,2,3,1" },
                { "Gambling Dens", "1,2,3,0" },
                { "Narcotics Distribution", "1,2,1,2" },
                { "CBRNE Dealing", "1,0,4,1" },
                { "Ivory Poaching", "3,0,2,1" },
                { "Murder Inc", "1,3,1,1" },
                //Doomsday Missions
                { "Crash Wall Street", "0,6,8,1" },
                { "Destroy IXP's", "2,6,6,1" },
                { "Bring Down Satellites", "2,2,7,4" },
                { "Kidnap Military & Political Leaders", "3,6,3,3" },
                { "Deploy Neurotoxin", "4,3,6,2" },
                { "Acquire Russian Nuclear Stockpile", "6,5,4,0" },
                { "Burgle Fort Knox", "1,8,4,2" },
                { "Supplant Major African Warlords", "8,4,1,2" },
                { "Take Over South American Cartels", "4,8,1,2" },
                { "Hijack Major World Media Outlets", "0,2,7,6" },
                { "Infiltrate Asian Intelligence Agencies", "3,4,4,4" },
                { "Corrupt the U.N.", "0,2,5,8" },
                //Agent Headquarters
                { "CIA Building", "2,2,2,2"},
                { "Interpol HQ", "1,2,2,1"}
            };

        private Dictionary<string, Dictionary<string, int>> flipMissionDictionary = new Dictionary<string, string> //resourceLookupDictionary <TileName, TileStats>
            {
                { "Blood Diamond Harvest", "3,2,2,0" },
                { "Skin Trade", "1,1,2,3" },
                { "Social Engineering Scams", "0,1,3,1" },
                { "Counterfeiting Operation", "0,3,2,0" },
                { "Ponzi Schemes", "0,1,3,2" },
                { "Political Corruption", "0,1,2,2" },
                { "Hacker Cell", "0,3,4,0" },
                { "Art Thievery", "0,3,3,1" },
                { "Exotic Car GTA", "0,3,3,0" },
                { "Smuggling Ring", "0,2,2,2" },
                { "Arms Dealing", "1,3,1,0" },
                { "Maritime Piracy", "3,2,2,0" },
                { "Rig Sports Events", "0,1,2,2" },
                { "Gambling Dens", "1,1,3,0" },
                { "Narcotics Distribution", "1,3,2,1" },
                { "CBRNE Dealing", "1,0,4,2" },
                { "Ivory Poaching", "3,2,2,0" },
                { "Murder Inc", "2,4,1,0"}
            };


        //parses the status strings into a dictionary
        public Dictionary<string, int> ParseToDictionary(string dataStrip)
        {
            string[] keyArray = new string[] { "Combat", "Stealth", "Cunning", "Diplomacy" };
            string[] stringArray = dataStrip.Split(',');
            int[] statusValueArray = Array.ConvertAll(stringArray, int.Parse);
            Dictionary<string, int> tileStatus = new Dictionary<string, int>();

            if (keyArray.Length == statusValueArray.Length)
            {
                for (int i = 0; i < keyArray.Length; i++)
                {
                    tileStatus.Add(keyArray[i], statusValueArray[i]);
                }
            }

            return tileStatus;
        }


        public Dictionary<string, int> tileIndex(string tileName, Boolean flipped) //returns Dictionary of matching name
        {
            if (flipped == true)
            {
                Dictionary<string, int> tileIndex = flipMissionDictionary[tileName];
            }
            else if (flipped == false)
            {
                Dictionary<string, int> tileIndex = missionDictionary[tileName];
            }
            else
            {
                //throw some exception
                System.ArgumentException argEx = new System.ArgumentException("Index is out of range", "index", ex);
                throw argEx;
            }
            return tileIndex;
        }

        public Dictionary<string, int> CalculateTileStats(int alerts, string tileName, Dictionary<string, Dictionary<string, int>> tileData)
        {
            Dictionary<string, int> tileStats = tileData[tileName];

            if (alerts > 0)
            {
                tileStats["Combat"] *= alerts;
                tileStats["Stealth"] *= alerts;
                tileStats["Cunning"] *= alerts;
                tileStats["Diplomacy"] *= alerts;
            }

            return tileStats;
        }

    }
}