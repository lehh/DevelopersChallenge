using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChampionshipManager.Model
{
    [Table("Team")]
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ISet<TeamChampionship> TeamChampionships { get; set; }
    }
}