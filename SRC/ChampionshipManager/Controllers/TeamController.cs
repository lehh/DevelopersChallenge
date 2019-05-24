using ChampionshipManager.Model;
using ChampionshipManager.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChampionshipManager.Controllers
{
    public class TeamController : Controller
    {
        private readonly ChampionshipManagerContext _context;

        public TeamController(ChampionshipManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(TeamCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var team = new Team()
                    {
                        Name = model.Name
                    };

                    _context.Team.Add(team);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Team " + team.Name + " inserted successfully!";

                    return View();
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }

            return View(model);
        }
    }
}