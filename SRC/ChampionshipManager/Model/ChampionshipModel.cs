using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChampionshipManager.Model
{
    [Table("Championship")]
    public class Championship
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //Tells if the Championship is In Progress or Finished.
        public bool Active { get; set; }

        public ISet<TeamChampionship> TeamsChampionship { get; set; }
    }
}