using Globomatics.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globomatics.Controllers
{
    //TODO: adding proposals with encryption
    public class ProposalController : Controller
    {
        private readonly IConferenceService conferenceService;
        private readonly IProposalService proposalService;
        private readonly IDataProtector protector;
        private readonly ILogger<ProposalController> logger;
        public ProposalController(ILogger<ProposalController> logger,
            IConferenceService conferenceService, IProposalService proposalService,
            IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants)
        {
            protector = protectionProvider.CreateProtector(purposeStringConstants.ConferenceIdQueryString);
            this.logger = logger;
            this.conferenceService = conferenceService;
            this.proposalService = proposalService;
        }

        public async Task<IActionResult> Index(string conferenceId)
        {
            var decryptedConferenceId =int.Parse(protector.Unprotect(conferenceId));
            var conference = await conferenceService.GetById(decryptedConferenceId);
            ViewBag.Title = $"Proposals for conference {conference.Name}, {conference.Location}";
            ViewBag.ConferenceId = conferenceId;

            return View(await proposalService.GetAll(decryptedConferenceId));
        }

        public IActionResult Add(string conferenceId)
        {
            ViewBag.Title = "Add new proposal";
            var confId = int.Parse(protector.Unprotect(conferenceId));
            logger.Log(LogLevel.Information, $"conferenceID: {confId}");
            ViewBag.ConferenceId = confId;
            ViewBag.EncryptedConferenceId = conferenceId;
            return View(new ProposalModel
            {
                ConferenceId = confId,
                EncryptedConferenceId = conferenceId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProposalModel model)
        {
            if (ModelState.IsValid)
            {
                await proposalService.Add(model);
            }
            return RedirectToAction("Index", new { conferenceId = protector.Protect(model.ConferenceId.ToString())});
        }

        public async Task<IActionResult> Approve(int proposalId)
        {
            var proposal = await proposalService.Approve(proposalId);
            return RedirectToAction("Index", new { conferenceId = protector.Protect(proposal.ConferenceId.ToString())});
        }

    }
}
