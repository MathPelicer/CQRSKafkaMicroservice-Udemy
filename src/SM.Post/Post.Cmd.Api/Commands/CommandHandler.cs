using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Stores;

namespace Post.Cmd.Api.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourceHandler<PostAggregate> eventSourceHandler;

        public CommandHandler(IEventSourceHandler<PostAggregate> eventSourceHandler)
        {
            this.eventSourceHandler = eventSourceHandler;
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            var aggregate = await eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Comment, command.Username);

            await eventSourceHandler.SaveAsync(aggregate);

        }

        public async Task HandleAsync(DeletePostCommand command)
        {
            var aggregate = await eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.Username);

            await eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            var aggregate = await eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.CommentId, command.Comment, command.Username);

            await eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            var aggregate = await eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Message);

            await eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            var aggregate = await eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();

            await eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(NewPostCommand command)
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);

            await eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            var aggregate = await eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.CommentId, command.Username);

            await eventSourceHandler.SaveAsync(aggregate);
        }
    }
}
