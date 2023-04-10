using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public enum FeedbackStatus
    {
        Open = 0,
        InProgress = 1,
        Resolved = 2,
        Closed = 3
    }




    public static class FeedbackStatusExtensions
    {
        public static FeedbackStatus FromString(string status)
        {
            switch (status.ToLower())
            {
                case "open":
                    return FeedbackStatus.Open;
                case "InProgress":
                    return FeedbackStatus.InProgress;
                case "Resolved":
                    return FeedbackStatus.InProgress;
                case "closed":
                    return FeedbackStatus.Closed;
                default:
                    throw new ArgumentException($"Unknown feedback status: {status}");
            }
        }
    }


}