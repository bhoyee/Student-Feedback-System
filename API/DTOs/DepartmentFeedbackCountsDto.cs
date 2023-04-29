using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class DepartmentFeedbackCountsDto
    {
        public int TotalFeedbacks { get; set; }
        public int TotalOpenFeedbacks { get; set; }
        public int TotalClosedFeedbacks { get; set; }
        public int TotalInProgress {get; set;}
        public int ResolvedFeedback { get; set; }
    }
}