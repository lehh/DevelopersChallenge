using ChampionshipManager.Model;
using ChampionshipManager.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error: " + ex.Message;
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var team = await _context.Team.FindAsync(id.Value);

                    if (team != null)
                    {
                        _context.Entry(team).Collection(t => t.TeamChampionships).Load();

                        if (team.TeamChampionships.Count != 0)
                            throw new Exception("You can't delete a team that's already in a championship.");

                        using (_context)
                        {
                            _context.Entry(team).State = EntityState.Deleted;
                            await _context.SaveChangesAsync();
                        }

                        TempData["Message"] = "Team "+ team.Name +" deleted successfully!";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error: " + ex.Message;
                }

                return RedirectToAction("Index", "Home");
            }

            return BadRequest();
        }
    }
}