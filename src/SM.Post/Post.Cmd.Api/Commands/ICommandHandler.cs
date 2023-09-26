using Post.Cmd.Api.Commands;

namespace Post.Cmd.Infrastructure.Stores
{
    public interface ICommandHandler
    {
        Task HandleAsync(AddCommentCommand command);
        Task HandleAsync(DeletePostCommand command);
        Task HandleAsync(EditCommentCommand command);
        Task HandleAsync(EditMessageCommand command);
        Task HandleAsync(LikePostCommand command);
        Task HandleAsync(NewPostCommand command);
        Task HandleAsync(RemoveCommentCommand command);
    }
}
