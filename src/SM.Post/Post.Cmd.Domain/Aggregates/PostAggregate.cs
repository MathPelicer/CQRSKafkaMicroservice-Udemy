using CQRS.Core.Domain;
using CQRS.Core.Messages;
using Post.Commun.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();
    
        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate() 
        { 
        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                PostedAt = DateTime.Now
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            Id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit a message of a inactive post.");
            }

            if(string.IsNullOrEmpty(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty.");
            }

            RaiseEvent(new MessageUpdatedEvent
            {
                Id = Id,
                Message = message,
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            Id = @event.Id;
        }

        public void LikePost()
        {
            if(!_active)
            {
                throw new InvalidOperationException("You cannot like a message of a inactive post.");
            }

            RaiseEvent(new PostLikedEvent 
            {
                Id = Id 
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            Id = @event.Id;
        }

        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add comment to an inactive post.");
            }

            if (string.IsNullOrEmpty(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty.");
            }

            RaiseEvent(new CommentAddedEvent
            {
                Id = Id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                AddedAt = DateTime.Now
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            Id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (_active)
            {
                throw new InvalidOperationException("You cannot add edit a comment in an inactive post.");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase)) 
            {
                throw new InvalidOperationException("You cannot edit a comment made by other user.");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                Id = Id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditedAt = DateTime.Now
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            Id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (_active)
            {
                throw new InvalidOperationException("You cannot remove a comment in an inactive post.");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You cannot remove a comment made by other user.");
            }

            RaiseEvent(new CommentRemovedEvent
            {
                Id = Id,
                CommentId = commentId,
                
            });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            Id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username) 
        {
            if (_active)
            {
                throw new InvalidOperationException("The post has already been removed.");
            }

            if(!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a post made by other user.");
            }

            RaiseEvent(new PostRemovedEvent{
                Id = Id
            });
        }

        public void Apply(PostRemovedEvent @event)
        {
            Id = @event.Id;
            _active = false;
        }
    }
}
