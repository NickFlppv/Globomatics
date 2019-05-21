using Microsoft.AspNetCore.DataProtection;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globomatics.Services
{
    public class ProposalMemoryService : IProposalService
    {
        private readonly List<ProposalModel> proposals = new List<ProposalModel>();
        private readonly IDataProtector protector;
        public ProposalMemoryService(IDataProtectionProvider protectionProvider,
            PurposeStringConstants purposeStringConstants)
        {
            protector = protectionProvider.CreateProtector(purposeStringConstants.ConferenceIdQueryString);
            proposals.Add(new ProposalModel
            {
                Id = 1,
                ConferenceId = 1,
                EncryptedConferenceId = protector.Protect("1"),
                Speaker = "Nikolay Filippov",
                Title = "ASP.NET Core 2.x"
            });
            proposals.Add(new ProposalModel
            {
                Id = 2,
                ConferenceId = 2,
                EncryptedConferenceId = protector.Protect("2"),
                Speaker = "John Doe",
                Title = "Understanding C#"
            });
        }

        public Task Add(ProposalModel model)
        {
            model.Id = proposals.Max(p => p.Id) + 1;
            proposals.Add(model);
            return Task.CompletedTask;
        }

        public Task<ProposalModel> Approve(int proposalId)
        {
            return Task.Run(() =>
            {
                var proposal = proposals.First(p => p.Id == proposalId);
                proposal.Approved = true;
                return proposal;
            });
        }

        public Task<IEnumerable<ProposalModel>> GetAll(int conferenceId)
        {
            return Task.Run(() => proposals.Where(c => c.ConferenceId == conferenceId));
        }
    }
}
