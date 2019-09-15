using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public interface IModel : IValidatable, IHasTimestamp
    {
    }
}
