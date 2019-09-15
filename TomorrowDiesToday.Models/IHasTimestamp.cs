using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public interface IHasTimestamp
    {
        DateTime Timestamp { get; }
    }
}
