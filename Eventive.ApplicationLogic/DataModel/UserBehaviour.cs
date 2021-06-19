using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventive.ApplicationLogic.DataModel
{
    public class UserBehaviour
    {
        public Guid Id { get; set; }
        public Participant Participant { get; set; }
        [NotMapped]
        public Dictionary<EventOrganized, double> ProximityScore { get; set; }
        [NotMapped]
        public Dictionary<EventOrganized, double> CategoryScore { get; set; }
        public DateTime LastUpdated { get; set; }

        public static UserBehaviour Create(Dictionary<EventOrganized, double> proximityScore,
            Dictionary<EventOrganized, double> categoryScore, Participant participant)
        {
            var newUserBehaviour = new UserBehaviour()
            {
                Id = Guid.NewGuid(),
                ProximityScore = proximityScore,
                CategoryScore = categoryScore,
                Participant = participant,
                LastUpdated = DateTime.Now
            };

            return newUserBehaviour;
        }

        public UserBehaviour Update(Dictionary<EventOrganized, double> proximityScore,
            Dictionary<EventOrganized, double> categoryScore)
        {
            ProximityScore = proximityScore;
            CategoryScore = categoryScore;
            LastUpdated = DateTime.Now;
            return this;
        }
    }
}
