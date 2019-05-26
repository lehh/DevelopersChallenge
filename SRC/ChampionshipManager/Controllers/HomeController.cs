using ChampionshipManager.Model;
using ChampionshipManager.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace ChampionshipManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChampionshipManagerContext _context;

        public HomeController(ChampionshipManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var championshipsList = await _context.Championship.ToListAsync();
            var teamList = await _context.Team.ToListAsync();

            var model = new HomeIndexViewModel();

            foreach (var championship in championshipsList)
            {
                model.ChampionshipList.Add(new HomeIndexViewModel.ChampionshipData()
                {
                    Id = championship.Id,
                    Name = championship.Name,
                    Active = championship.Active
                });
            }

            foreach (var team in teamList)
            {
                model.TeamList.Add(new HomeIndexViewModel.TeamData()
                {
                    Id = team.Id,
                    Name = team.Name
                });
            }

            return View(model);
        }
    }
}