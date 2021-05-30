using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public Participant Commenter { get; set; }
        public Guid EventOrganizedId { get; set; }

        public static Comment Create(Participant commenter, Guid organizedEventId, string message)
        {
            var newComment = new Comment()
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.Now,
                Commenter = commenter,
                EventOrganizedId = organizedEventId,
                Message = message
            };

            return newComment;
        }
    }
}
