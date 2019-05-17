using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Models
{
    public class StatisticsModel
    {
        [Display(Name ="Averege amount of attendees on conference")]
        public int AverageConferenceAttendees { get; set; }
        [Display(Name ="Number of Attendees on conferences")]
        public int NumberOfAttendees { get; set; }
    }
}
