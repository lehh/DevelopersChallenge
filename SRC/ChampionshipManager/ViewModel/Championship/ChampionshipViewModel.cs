using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChampionshipManager.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChampionshipManager.ViewModel
{
    public abstract class ChampionshipViewModel
    {
        [Required(ErrorMessage = "Please, fill the name field")]
        [Display(Name = "Name")]
        [StringLength(150)]
        public string Name { get; set; }

        public List<int> TeamIdList { get; set; }

        [Required(ErrorMessage = "Please, select at least 2 teams")]
        [Display(Name = "Teams")]
        public MultiSelectList TeamMultiSelectList { get; set; }
    }

    public class ChampionshipCreateViewModel : ChampionshipViewModel
    {

    }
}