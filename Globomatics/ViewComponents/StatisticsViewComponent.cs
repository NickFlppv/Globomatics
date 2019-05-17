using Globomatics.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globomatics.ViewComponents
{
    public class StatisticsViewComponent : ViewComponent
    {
        private readonly IConferenceService conferenceService;

        public StatisticsViewComponent(IConferenceService conferenceService)
        {
            this.conferenceService = conferenceService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var statistics = await conferenceService.GetStatistics();
            return View(statistics);
        }
    }
}
