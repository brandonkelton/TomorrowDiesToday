using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TomorrowDiesToday.Models.Enums
{
    public enum ErrorType
    {
        [Description("None")]
        None,

        [Description("Invalid Armament Count")]
        InvalidArmamentCount,

        [Description("Invalid Armament Type")]
        InvalidArmamentType,

        [Description("Invalid Hypnotic Spray Count")]
        InvalidHypnoticSprayCount,

        [Description("Invalid Named Henchman Count")]
        InvalidNamedHenchmanCount,

        [Description("Invalid Squad Size")]
        InvalidSquadSize,

        [Description("Invalid Tile Deactivation")]
        InvalidTileDeactivation,

        [Description("Invalid Tile Flip")]
        InvalidTileFlip,
    }
}
