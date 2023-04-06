using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class FeedbackReplyUpdateDto
    {
        public int id { get; set; }
        public string Message { get; set; }
        public bool IsPublic { get; set; }
    }
}