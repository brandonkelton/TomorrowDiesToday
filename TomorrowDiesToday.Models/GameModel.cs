using System;

namespace TomorrowDiesToday.Models
{
    public class GameModel : IModel
    {
        public bool IsValid()
        {
            return false;
        }

        public DateTime Timestamp { get; private set; }
    }
}
