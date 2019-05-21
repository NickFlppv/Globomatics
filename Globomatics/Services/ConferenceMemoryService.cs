using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shared.Models;

namespace Globomatics.Services
{
    public class ConferenceMemoryService : IConferenceService
    {
        private readonly List<ConferenceModel> conferences = new List<ConferenceModel>();
        private readonly IDataProtector protector;
        public ConferenceMemoryService(IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants)
        {
            this.protector = protectionProvider.CreateProtector(purposeStringConstants.ConferenceIdQueryString);
            conferences.Add(new ConferenceModel { Id = 1, EncryptedId = protector.Protect("1"), Name = "NDC", Location = "Berlin", Start = DateTime.Now.Date, AttendeeTotal = 1000 });
            conferences.Add(new ConferenceModel { Id = 2, EncryptedId = protector.Protect("2"), Name = "IT/DevConnections", Location = "Moscow", Start = DateTime.Now + TimeSpan.FromDays(200), AttendeeTotal = 10000 });
        }
        public Task Add(ConferenceModel model)
        {
            model.Id = conferences.Max(c => c.Id) + 1;
            model.EncryptedId = protector.Protect(model.Id.ToString());
            conferences.Add(model);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ConferenceModel>> GetAll()
        {
            return Task.Run(() => conferences.AsEnumerable());
        }

        public Task<ConferenceModel> GetById(int id)
        {
            return Task.Run(() => conferences.First(c => c.Id == id));
        }

        public Task<StatisticsModel> GetStatistics()
        {
            return Task.Run(() =>
            {
                return new StatisticsModel
                {
                    NumberOfAttendees = conferences.Sum(c => c.AttendeeTotal),
                    AverageConferenceAttendees = (int)conferences.Average(c => c.AttendeeTotal)
                };
            });
        }
    }
}
