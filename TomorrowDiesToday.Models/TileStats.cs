using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class TileStats
    {
        public int Combat { get; set; }

        public int Diplomacy { get; set; }

        public List<PlayerModel> Players = new List<PlayerModel>();

        public void Test()
        {
            var points = Players.Sum(player => {
                var calculation = someCalc(player.Points);
                return calculation;
            });
            var allPlayersquads = Players.SelectMany(player => player.Squads.Where(squad => squad.IsSelected));
        }
    }
}
