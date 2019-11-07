using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class SquadModel : IModel
    {
        public string PlayerId { get; set; }

        public string SquadId { get; set; }

        // Wasn't sure if you need an initial henchman of none (maybe don't create list with initial henchman?)
        // In that case, remove None as an ArmamentType
        public List<Armament> Armaments { get; set; } = new List<Armament>() { new Armament(ArmamentType.None) };

        public List<Stat> Stats { get; set; } = new List<Stat>();

        public bool IsSelected { get; set; } = false;

        public SquadModel()
        {
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

            //// Initialize Stats Dictionary
            Stats = new Dictionary<string, int>();
            Stats.Add("Combat", 0);
            Stats.Add("Stealth", 0);
            Stats.Add("Cunning", 0);
            Stats.Add("Diplomacy", 0);
        }
    }
}
