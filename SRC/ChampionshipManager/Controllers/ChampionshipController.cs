using ChampionshipManager.Model;
using ChampionshipManager.ViewModel;
using ChampionshipManager.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChampionshipManager.Controllers
{
    public class ChampionshipController : Controller
    {
        private readonly ChampionshipManagerContext _context;

        public ChampionshipController(ChampionshipManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new ChampionshipCreateViewModel();
            var teamList = await _context.Team.ToListAsync();

            foreach (var team in teamList)
            {
                model.TeamSelectList.Add(new SelectListItem()
                {
                    Text = team.Name,
                    Value = team.Id.ToString()
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ChampionshipCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                #region Rules
                var countSelectedTeams = model.TeamIdList.Count;

                if (!Tools.IsPowerOfTwo(countSelectedTeams))
                {
                    TempData["Message"] = "Error: The number of selected teams must be a power of 2.";
                    return View(model);
                }
                #endregion

                try
                {
                    #region Filling TeamSelectList
                    var teamList = await _context.Team.ToListAsync();

                    foreach (var team in teamList)
                    {
                        model.TeamSelectList.Add(new SelectListItem()
                        {
                            Text = team.Name,
                            Value = team.Id.ToString()
                        });
                    }
                    #endregion

                    var championship = new Championship() { Name = model.Name };
                    var teamChampionshipList = new List<TeamChampionship>();

                    var positionHash = Tools.RandomizeBrackets(model.TeamIdList);
                    var counter = 0;

                    foreach (var position in positionHash)
                    {
                        teamChampionshipList.Add(new TeamChampionship()
                        {
                            Championship = championship,
                            Team = await _context.Team.FindAsync(model.TeamIdList[counter]),
                            TreePosition = position,
                            Level = (int)Math.Sqrt(countSelectedTeams)
                        });

                        counter++;
                    }

                    using (_context)
                    {
                        _context.TeamChampionship.AddRange(teamChampionshipList);
                        await _context.SaveChangesAsync();
                    }

                    TempData["Message"] = "Championship " + championship.Name + " inserted successfully!";

                    return View(model);
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
            }

            return View(model);
        }

        public ActionResult Manage(int? id)
        {
            if (id.HasValue)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}