using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChampionshipManager.Model
{
    [Table("Championship")]
    public class Championship
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public ISet<TeamChampionship> TeamsChampionship { get; set; }
    }
}