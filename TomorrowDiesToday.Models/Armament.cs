using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class Armament
    {
        public ArmamentType ArmamentType { get; private set; }

        public int Count { get; private set; } = 0;

        public ArmamentStats Stats;

        public Armament(ArmamentType armamentType)
        {
            ArmamentType = armamentType;
        }

        public void SetCount(int value)
        {
            Count = value;
        }
    }
}
