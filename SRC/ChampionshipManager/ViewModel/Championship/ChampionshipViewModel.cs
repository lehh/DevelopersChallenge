using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChampionshipManager.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChampionshipManager.ViewModel
{
    public abstract class ChampionshipViewModel
    {
        public ChampionshipViewModel()
        {
            TeamSelectList = new List<SelectListItem>();
        }

        [Required(ErrorMessage = "Please, fill the name field")]
        [Display(Name = "Name")]
        [StringLength(150)]
        public string Name { get; set; }

        public List<int> TeamIdList { get; set; }

        [Required(ErrorMessage = "Select at least 2 teams.")]
        [Display(Name = "Teams")]
        public List<SelectListItem> TeamSelectList { get; set; }
    }

    public class ChampionshipCreateViewModel : ChampionshipViewModel
    {

    }

    public class ChampionshipManageViewModel
    {
        public ChampionshipManageViewModel()
        {
            TeamDataList = new List<TeamData>();
        }

        public struct TeamData
        {   
            //Team Id
            public int Id { get; set; }
            public string Name { get; set; }
            public int TreePosition { get; set; }
            public int Level { get; set; }
        }

        //Championship Id
        public int Id { get; set; }

        public List<TeamData> TeamDataList { get; set; }

    }
}