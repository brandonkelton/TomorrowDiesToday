using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TomorrowDiesToday.Models.Enums
{
    public enum StatType
    {
        [Description("Combat")]
        Combat,

        [Description("Stealth")]
        Stealth,

        [Description("Cunning")]
        Cunning,

        [Description("Diplomacy")]
        Diplomacy
    }
}
