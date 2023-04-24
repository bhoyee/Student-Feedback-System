using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class FeedbackCountsDto
    {
        public int TotalFeedbacks { get; set; }
        public int OpenFeedbacks { get; set; }
        public int ClosedFeedbacks { get; set; }
    }
}