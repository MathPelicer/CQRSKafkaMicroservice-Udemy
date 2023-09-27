using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreateAsync(PostEntity postEntity);

        Task UpdateAsync(PostEntity postEntity);

        Task DeleteAsync(Guid id);

        Task<PostEntity> GetByIdAsync(Guid id);

        Task<List<PostEntity>> GetAllAsync();

        Task<List<PostEntity>> GetAllByAuthorAsync(string author);

        Task<List<PostEntity>> GetAllByLikesAsync(int likes);

        Task<List<PostEntity>> GetAllByCommentsAsync(int likes);

    }
}
