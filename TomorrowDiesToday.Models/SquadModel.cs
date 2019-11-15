using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Models
{
    public class SquadModel : IModel
    {
        public string PlayerId { get; set; }

        public string SquadId { get; set; }

        public List<Armament> Armaments { get; set; } = new List<Armament>();
        public List<Armament> Abilities { get; set; } = new List<Armament>();

        public SquadStats Stats { get; set; } = new SquadStats();

        public bool IsSelected { get; set; }

        public SquadModel()
        {
            // Initialize Armaments
            Armaments.Add(new Armament(ArmamentType.Thief, new ArmamentStats(0, 2, 1, 0)));
            Armaments.Add(new Armament(ArmamentType.Hacker, new ArmamentStats(0, 1, 2, 0)));
            Armaments.Add(new Armament(ArmamentType.Soldier, new ArmamentStats(2, 1, 0, 0)));
            Armaments.Add(new Armament(ArmamentType.Assassin, new ArmamentStats(1, 2, 0, 0)));
            Armaments.Add(new Armament(ArmamentType.Fixer, new ArmamentStats(0, 0, 1, 2)));
            Armaments.Add(new Armament(ArmamentType.Scientist, new ArmamentStats(0, 0, 2, 1)));
            Armaments.Add(new Armament(ArmamentType.HypnoticSpray, new ArmamentStats(0, 0, 0, 2)));
            Armaments.Add(new Armament(ArmamentType.ExplosiveRounds, new ArmamentStats(2, 0, 0, 0)));
            Abilities.Add(new Armament(ArmamentType.UgoCombat, new ArmamentStats(1, 0, 0, 0)));
            Abilities.Add(new Armament(ArmamentType.UgoStealth, new ArmamentStats(0, 1, 0, 0)));
            Abilities.Add(new Armament(ArmamentType.UgoCunning, new ArmamentStats(0, 0, 1, 0)));
            Abilities.Add(new Armament(ArmamentType.UgoDiplomacy, new ArmamentStats(0, 0, 0, 1)));
        }
    }
}
