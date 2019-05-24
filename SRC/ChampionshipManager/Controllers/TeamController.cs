using Microsoft.AspNetCore.Mvc;
using ChampionshipManager.Model;

namespace ChampionshipManager.Controllers
{
    public class TeamController : Controller
    {
        private readonly ChampionshipManagerContext _context;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            return View();
        }
    }
}