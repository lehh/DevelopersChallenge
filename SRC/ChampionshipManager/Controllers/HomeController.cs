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

            var model = new HomeIndexViewModel();

            foreach (var championship in championshipsList)
            {
                model.ChampionshipList.Add(new HomeIndexViewModel.ChampionshipData()
                {
                    Name = championship.Name
                });
            }

            return View(model);
        }
    }
}