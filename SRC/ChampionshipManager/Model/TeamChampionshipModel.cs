using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChampionshipManager.Model
{
    [Table("Team_Championship")]
    public class TeamChampionship
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int ChampionshipId { get; set; }
        public Championship Championship { get; set; }

        //The zero based binary tree index.
        public int TreePosition { get; set; }

        //Tells if the Team is out or active.
        public bool TeamActive { get; set; }
    }
}