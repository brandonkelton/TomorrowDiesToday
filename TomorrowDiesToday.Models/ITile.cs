using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    interface ITile
    {
        bool isToggled { get; set; }
        int alertTokens { get; set; }
        string imageLocation { get; set; }
        Dictionary<string, int> tileStats { get; set; }

        bool FlipToggle(bool isToggled);
        int IncrementCounter();
        int DecrementCounter();

    }
}
