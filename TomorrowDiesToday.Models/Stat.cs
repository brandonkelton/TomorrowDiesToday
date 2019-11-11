
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Models
{
    public class Stat
    {
        public StatType StatType { get; private set; }

        public int Value { get; private set; } = 0;

        public Stat(StatType statType)
        {
            StatType = statType;
        }

        public void SetValue(int value)
        {
            Value = value;
        }
    }
}