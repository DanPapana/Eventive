using Eventive.ApplicationLogic.DataModel;
using System.Collections.Generic;

namespace Eventive.Models.Events
{
    public class CommentsViewModel
    {
        public IEnumerable<Comment> Comments { get; set; }
    }
}
