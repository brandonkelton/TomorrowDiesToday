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

        [Description("Invalid Alert Token Count")]
        InvalidAlertTokenCount,

        [Description("Invalid Ability Count")]
        InvalidAbilityCount,

        [Description("Invalid Ability Type")]
        InvalidAbilityType,

        [Description("Invalid Armament Count")]
        InvalidArmamentCount,

        [Description("Invalid Armament Type")]
        InvalidArmamentType,

        [Description("Invalid Item Count")]
        InvalidItemCount,

        [Description("Invalid Item Type")]
        InvalidItemType,

        [Description("Invalid Named Henchman Count")]
        InvalidNamedHenchmanCount,

        [Description("Invalid Named Henchman Type")]
        InvalidNamedHenchmanType,

        [Description("Invalid Squad Size")]
        InvalidSquadSize,

        [Description("Invalid Tile Deactivation")]
        InvalidTileDeactivation,

        [Description("Invalid Tile Flip")]
        InvalidTileFlip,
    }
}
