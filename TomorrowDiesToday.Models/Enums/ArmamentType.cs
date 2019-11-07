using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public enum ArmamentType
    {
        [Description("No Henchman Selected")]
        None,

        [Description("Archibald Kluge")]
        ArchibaldKluge,

        [Description("Axle Robbins")]
        AxleRobbins,

        [Description("Azura Badeau")]
        AzuraBadeau,

        [Description("Boris \"Myasneek\"")]
        BorisMyasneek,

        [Description("Cassandra O'Shea")]
        CassandraOShea,

        [Description("Emmerson Barlow")]
        EmmersonBarlow,

        [Description("Jin Feng")]
        JinFeng,

        [Description("The Node")]
        TheNode,

        [Description("Ugo Dottore")]
        UgoDottore,

        [Description("Thief")]
        Thief,

        [Description("Hacker")]
        Hacker,

        [Description("Soldier")]
        Soldier,

        [Description("Assassin")]
        Assassin,

        [Description("Fixer")]
        Fixer,

        [Description("Scientist")]
        Scientist,

        [Description("Hypnotic Spray")]
        HypnoticSpray,

        [Description("Explosive Rounds")]
        ExplosiveRounds,

        [Description("Ugo Combat")]
        UgoCombat,

        [Description("Ugo Stealth")]
        UgoStealth,

        [Description("Ugo Cunning")]
        UgoCunning,

        [Description("Ugo Diplomacy")]
        UgoDiplomacy
    }
}
