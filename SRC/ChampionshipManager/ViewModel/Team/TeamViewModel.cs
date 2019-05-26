using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChampionshipManager.ViewModel
{
    public abstract class TeamViewModel
    {
        [Required(ErrorMessage = "Please, fill the Name field.")]
        [Display(Name = "Team Name")]
        [MaxLength(150)]
        public string Name { get; set; }
    }

    public class TeamCreateViewModel : TeamViewModel
    {

    }

    public class TeamUpdateViewModel : TeamViewModel
    {
        public int Id { get; set; }
    }
}