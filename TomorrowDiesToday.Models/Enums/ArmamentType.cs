using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TomorrowDiesToday.Models.Enums
{
    public enum ArmamentType
    {
        [Description("No Armament Selected")]
        None = 0,

        #region Named Henchmen

        [Description("General Goodman")]
        GeneralGoodman,

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

        #endregion

        #region Faceless Henchmen

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

        #endregion

        #region Abilities

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
        UgoDiplomacy,

        #endregion
    }
}
