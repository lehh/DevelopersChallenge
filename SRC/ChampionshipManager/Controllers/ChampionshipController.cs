using ChampionshipManager.Model;
using ChampionshipManager.ViewModel;
using ChampionshipManager.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace ChampionshipManager.Controllers
{
    public class ChampionshipController : Controller
    {
        //Database Context
        private readonly ChampionshipManagerContext _context;

        public ChampionshipController(ChampionshipManagerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Action which returns the Create View along with the available teams.
        /// </summary>
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

        /// <summary>
        /// Action which creates the Championship received by the model.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create(ChampionshipCreateViewModel model)
        {
            //Fill the Team SelectList with the available teams.
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

            if (ModelState.IsValid)
            {

                #region Validations
                var countSelectedTeams = model.TeamIdList.Count;

                //Verify if the number of selected teams is a power of two.
                if (!Tools.IsPowerOfTwo(countSelectedTeams))
                {
                    throw new Exception("The number of selected teams must be a power of 2.");
                }
                #endregion

                try
                {
                    var championship = new Championship() { Name = model.Name, Active = true };
                    var teamChampionshipList = new List<TeamChampionship>();

                    var positionHash = Tools.RandomizePositions(model.TeamIdList.Count);
                    var counter = 0;

                    //Creates a TeamChampionship for each position in the positionHash (positions = count of teams)
                    foreach (var position in positionHash)
                    {
                        teamChampionshipList.Add(new TeamChampionship()
                        {
                            Championship = championship,
                            Team = await _context.Team.FindAsync(model.TeamIdList[counter]), //Maybe there's a better way to do this?
                            TreePosition = position,
                            TeamActive = true
                        });

                        counter++;
                    }

                    using (_context)
                    {
                        _context.TeamChampionship.AddRange(teamChampionshipList);
                        await _context.SaveChangesAsync();
                    }

                    TempData["Message"] = "Championship " + championship.Name + " created successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error: " + ex.Message;
                }
            }

            return View(model);
        }

        /// <summary>
        /// Delete a Championship by id.
        /// </summary>
        /// <param name="id">Championship Id</param>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var championship = await _context.Championship.FindAsync(id.Value);

                    if (championship != null)
                    {
                        using (_context)
                        {
                            _context.Entry(championship).State = EntityState.Deleted;
                            await _context.SaveChangesAsync();
                        }

                        TempData["Message"] = "Championship deleted successfully!";
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

        /// <summary>
        /// Action which returns the Manage page where the championship can be managed.
        /// </summary>
        /// <param name="id">Championship Id</param>
        public async Task<ActionResult> Manage(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    //Get championship by id.
                    var championship = await _context.Championship.SingleAsync(c => c.Id == id.Value);

                    if (championship != null)
                    {
                        //Load TeamsChampionship in the championship object.
                        _context.Entry(championship).Collection(c => c.TeamsChampionship).Load();

                        var model = new ChampionshipManageViewModel()
                        {
                            Id = id.Value,
                            Name = championship.Name,
                            Active = championship.Active
                        };

                        return View(model);
                    }

                    return NotFound();
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error: " + ex.Message;
                }
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Action which returns a Json object with the Championship Teams based on the Championship id.
        /// </summary>
        /// <param name="id"> Championship Id </param>
        public async Task<ActionResult> GetChampionshipTeams(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    //Get championship by id.
                    var championship = await _context.Championship.SingleAsync(c => c.Id == id.Value);

                    if (championship != null)
                    {
                        var teamChampList = new List<object>();

                        //Load TeamChampionship along with Teams in the championship object.
                        await _context.Entry(championship).Collection(c => c.TeamsChampionship)
                            .Query().Include(tc => tc.Team).ToListAsync();

                        foreach (var teamChamp in championship.TeamsChampionship)
                        {
                            teamChampList.Add(new
                            {
                                id = teamChamp.TeamId,
                                name = teamChamp.Team.Name,
                                treePosition = teamChamp.TreePosition,
                                active = teamChamp.TeamActive
                            });
                        }

                        return Json(teamChampList);
                    }

                    return NotFound();

                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(ex.Message);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Advance a team to the next championship phase.
        /// </summary>
        /// <param name="id">Championship Id</param>
        /// <param name="teamId">Team Id</param>
        [HttpPost]
        public async Task<ActionResult> AdvanceToNextPhase(int? id, int? teamId)
        {
            if (teamId.HasValue && id.HasValue)
            {
                try
                {
                    //Get championship by id.
                    var championship = await _context.Championship.SingleAsync(c => c.Id == id);

                    //Load TeamChampionship in the championship object.
                    _context.Entry(championship).Collection(c => c.TeamsChampionship).Load();

                    //Get the team that's going to the next phase.
                    var teamChamp = championship.TeamsChampionship.Where(tc => tc.TeamId == teamId).SingleOrDefault();

                    var teamPosition = teamChamp.TreePosition;

                    //Find the parent node index.
                    var newPosition = (teamPosition - 1) / 2; //The parent node of N is (N-1)/2.

                    if (newPosition <= 0)
                    {
                        newPosition = 0;

                        //If the team won the finals, the Championship ends.
                        championship.Active = false;
                    }

                    teamChamp.TreePosition = newPosition;

                    int siblingPosition;

                    //Find the node index of the defeated team.
                    if ((teamPosition - 1) % 2 == 0)
                        siblingPosition = teamPosition + 1;
                    else
                        siblingPosition = teamPosition - 1;

                    //Get the defeated team.
                    var siblingTeamChamp = championship.TeamsChampionship.Where(tc => tc.TreePosition == siblingPosition).SingleOrDefault();
                    //Disqualify them from the championship
                    siblingTeamChamp.TeamActive = false;

                    //Save changes to database.
                    using (_context)
                    {
                        _context.Entry(teamChamp).State = EntityState.Modified;
                        _context.Entry(siblingTeamChamp).State = EntityState.Modified;
                        _context.Entry(championship).State = EntityState.Modified;

                        await _context.SaveChangesAsync();
                    }

                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(ex.Message);
                }

                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// View which generates the html brackets based on the number of teams and total nodes.
        /// </summary>
        public ViewResult GenerateBrackets(int numberOfTeams, int totalNodes)
        {
            return View("DynBrackets", new ChampionshipBracketsViewModel() { NumberOfTeams = numberOfTeams, TotalNodes = totalNodes });
        }
    }
}