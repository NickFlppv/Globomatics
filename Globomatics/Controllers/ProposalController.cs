using Globomatics.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globomatics.Controllers
{
    public class ProposalController : Controller
    {
        private readonly IConferenceService conferenceService;
        private readonly IProposalService proposalService;

        public ProposalController(IConferenceService conferenceService, IProposalService proposalService)
        {
            this.conferenceService = conferenceService;
            this.proposalService = proposalService;
        }

        public async Task<IActionResult> Index(int conferenceId)
        {
            var conference = await conferenceService.GetById(conferenceId);
            ViewBag.Title = $"Proposals for conference {conference.Name}, {conference.Location}";
            ViewBag.ConferenceId = conferenceId;

            return View(await proposalService.GetAll(conferenceId));
        }

        public IActionResult Add(int conferenceId)
        {
            ViewBag.Title = "Add new proposal";
            return View(new ProposalModel
            {
                ConferenceId = conferenceId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProposalModel model)
        {
            if (ModelState.IsValid)
            {
                await proposalService.Add(model);
            }
            return RedirectToAction("Index", new { conferenceId = model.ConferenceId});
        }

        public async Task<IActionResult> Approve(int proposalId)
        {
            var proposal = await proposalService.Approve(proposalId);
            return RedirectToAction("Index", new { conferenceId = proposal.ConferenceId});
        }

    }
}
