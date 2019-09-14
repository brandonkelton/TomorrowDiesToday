using System;

namespace TomorrowDiesToday.Models
{
    public class GameModel : IModel
    {
        public bool IsValid { get; private set; }

        public DateTime LastUpdated { get; private set; }
    }
}
