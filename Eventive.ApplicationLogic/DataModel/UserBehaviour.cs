using System.Collections.Generic;
using static Eventive.ApplicationLogic.DataModel.EventOrganized;

namespace Eventive.ApplicationLogic.DataModel
{
    public class UserBehaviour
    {
        public Participant Participant { get; set; }
        public Dictionary<EventOrganized, double> EventScore { get; set; }
        public Dictionary<EventCategory, double> CategoryScore { get; set; }

        public static UserBehaviour Create(Dictionary<EventOrganized, double> eventScore,
            Dictionary<EventCategory, double> categoryScore, Participant participant)
        {
            var newUserBehaviour = new UserBehaviour()
            {
                EventScore = eventScore,
                CategoryScore = categoryScore,
                Participant = participant
            };

            return newUserBehaviour;
        }
    }
}
