
using System;
using System.Collections.Generic;

namespace TomorrowDiesToday.Services
{
    class SquadManagementService
    {

        public Dictionary<string, int> CalculateSquadStats(Dictionary<string, int> squadData)
        {
            Dictionary<string, int> squadStats;
            squadStats.Add("Combat", CalculateCombat(squadData));
            squadStats.Add("Stealth", CalculateStealth(squadData));
            squadStats.Add("Cunning", CalculateCunning(squadData));
            squadStats.Add("Diplomacy", CalculateDiplomacy(squadData));

            return squadStats;
        }

        #region Helper Methods
        private int CalculateCombat(Dictionary<string, int> squadData)
        {
            int total = 0;
            int facedHenchmen = squadData["Faced Henchmen"];
            int soldiers = squadData["Soldier"];
            int assassins = squadData["Assassin"];

            total += (soldiers * 2);
            total += assassins;

            switch (facedHenchmen)
            {
                //Axle Robbins
                case 2:
                    total += 1;
                    break;

                //Azura Badeau
                case 3:
                    total += 2;
                    break;

                //Boris "Myasneek"
                case 4:
                    total += 3;
                    break;

                //Emerson Barlow
                case 6:
                    total += 1;
                    break;

                //Ugo Dottore
                case 9:
                    total += 1;
                    break;

                default:
                    break;
            }

            if (facedHenchmen == 9)
            {
                total += squadData["Ugo Combat"];
            }

            if (squadData["Explosive Rounds"] == 1)
            {
                total += 2;
            }

            return total;
        }

        private int CalculateStealth(Dictionary<string, int> squadData)
        {
            int total = 0;
            int facedHenchmen = squadData["Faced Henchmen"];
            int soldiers = squadData["Soldier"];
            int assassins = squadData["Assassin"];
            int thieves = squadData["Thief"];
            int hackers = squadData["Hacker"];

            total += soldiers;
            total += (assassins * 2);
            total += (thieves * 2);
            total += hackers;

            switch (facedHenchmen)
            {
                //Archibald Kluge
                case 1:
                    total += 1;
                    break;

                //Azura Badeau
                case 3:
                    total += 2;
                    break;

                //Boris "Myasneek"
                case 4:
                    total += 1;
                    break;

                //Emmerson Barlow
                case 6:
                    total += 3;
                    break;

                //Jin Feng
                case 7:
                    total += 3;
                    break;

                //The Node
                case 8:
                    total += 2;
                    break;

                default:
                    break;
            }

            if (facedHenchmen == 9)
            {
                total += squadData["Ugo Stealth"];
            }

            return total;
        }

        private int CalculateCunning(Dictionary<string, int> squadData)
        {
            int total = 0;
            int facedHenchmen = squadData["Faced Henchmen"];
            int thieves = squadData["Thief"];
            int scientists = squadData["Scientist"];
            int fixers = squadData["Fixer"];
            int hackers = squadData["Hacker"];

            total += thieves;
            total += (scientists * 2);
            total += fixers;
            total += (hackers * 2);

            switch (facedHenchmen)
            {
                //Archibald Kluge
                case 1:
                    total += 3;
                    break;

                //"Axle" Robbins
                case 2:
                    total += 2;
                    break;

                //Azura Badeau
                case 3:
                    total += 1;
                    break;

                //Boris "Myasneek"
                case 4:
                    total += 1;
                    break;

                //Cassandra O'Shea
                case 5:
                    total += 2;
                    break;

                //Emmerson Barlow
                case 6:
                    total += 1;
                    break;

                //Jin Feng
                case 7:
                    total += 1;
                    break;

                //The Node
                case 8:
                    total += 2;
                    break;

                //Ugo Dottore
                case 9:
                    total += 3;
                    break;

                default:
                    break;
            }

            if (facedHenchmen == 9)
            {
                total += squadData["Ugo Cunning"];
            }

            return total;
        }
        private int CalculateDiplomacy(Dictionary<string, int> squadData)
        {
            int total = 0;
            int facedHenchmen = squadData["Faced Henchmen"];
            int scientists = squadData["Scientist"];
            int fixers = squadData["Fixer"];

            total += scientists;
            total += (fixers * 2);

            switch(facedHenchmen)
            {
                //Archibald Kluge
                case 1:
                    total += 1;
                    break;

                //"Axle" Robbins
                case 2:
                    total += 2;
                    break;

                //Cassandra O'Shea
                case 5:
                    total += 3;
                    break;

                //Jin Feng
                case 7:
                    total += 1;
                    break;

                //The Node
                case 8:
                    total += 1;
                    break;

                case 9:
                    total += 1;
                    break;

                default:
                    break;
            }

            if (squadData["Hypnotic Spray"] == 1)
            {
                total += 2;
            }

            if (facedHenchmen == 9)
            {
                total += squadData["Ugo Diplomacy"];
            }

            return total;
        }

        #endregion
    }
}

