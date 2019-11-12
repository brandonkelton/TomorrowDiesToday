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

        public List<Stat> Stats { get; set; } = new List<Stat>();

        public bool IsSelected { get; set; }

        public SquadModel()
        {
            // Initialize Armaments
            Armaments.Add(new Armament(ArmamentType.Thief));
            Armaments.Add(new Armament(ArmamentType.Hacker));
            Armaments.Add(new Armament(ArmamentType.Soldier));
            Armaments.Add(new Armament(ArmamentType.Assassin));
            Armaments.Add(new Armament(ArmamentType.Fixer));
            Armaments.Add(new Armament(ArmamentType.Scientist));
            Armaments.Add(new Armament(ArmamentType.HypnoticSpray));
            Armaments.Add(new Armament(ArmamentType.ExplosiveRounds));
            Armaments.Add(new Armament(ArmamentType.UgoCombat));
            Armaments.Add(new Armament(ArmamentType.UgoStealth));
            Armaments.Add(new Armament(ArmamentType.UgoCunning));
            Armaments.Add(new Armament(ArmamentType.UgoDiplomacy));

            // Initialize Stats List
            Stats.Add(new Stat(StatType.Combat));
            Stats.Add(new Stat(StatType.Stealth));
            Stats.Add(new Stat(StatType.Cunning));
            Stats.Add(new Stat(StatType.Diplomacy));
        }
    }
}
