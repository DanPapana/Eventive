using System;

namespace Eventive.ApplicationLogic.DataModel
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public User Commenter { get; set; }
    }
}
