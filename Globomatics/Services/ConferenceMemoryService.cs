using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Models;

namespace Globomatics.Services
{
    public class ConferenceMemoryService : IConferenceService
    {
        private readonly List<ConferenceModel> conferences = new List<ConferenceModel>();
        public ConferenceMemoryService()
        {
            conferences.Add(new ConferenceModel { Id = 1, Name = "NDC", Location = "Berlin", Start = DateTime.Now.Date, AttendeeTotal = 1000 });
            conferences.Add(new ConferenceModel { Id = 2, Name = "IT/DevConnections", Location = "Moscow", Start = DateTime.Now + TimeSpan.FromDays(200), AttendeeTotal = 10000 });
        }
        public Task Add(ConferenceModel model)
        {
            model.Id = conferences.Max(c => c.Id) + 1;
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
