using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class Armament
    {
        public ArmamentType ArmamentType { get; private set; }

        public int Value { get; private set; } = 0;

        public Armament(ArmamentType armamentType)
        {
            ArmamentType = armamentType;
        }

        public void SetValue(int value)
        {
            Value = value;
        }
    }
}
