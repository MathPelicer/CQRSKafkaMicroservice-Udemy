using CQRS.Core.Events;

namespace Post.Commun.Events
{
    public class PostUpdateEvent : BaseEvent
    {
        public PostUpdateEvent() : base(nameof(PostUpdateEvent))
        {

        }

        public string Message { get; set; }
    }
}
