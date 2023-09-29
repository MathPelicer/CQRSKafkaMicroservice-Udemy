using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _databaseContextFactory;

        public PostRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task CreateAsync(PostEntity postEntity)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Posts.Add(postEntity);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var post = await GetByIdAsync(id);

            if (post == null) return;

            context.Posts.Remove(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task<List<PostEntity>> GetAllAsync()
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                    .Include(p => p.Comments)
                    .ToListAsync();
        }

        public async Task<List<PostEntity>> GetAllByAuthorAsync(string author)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                    .Include(p => p.Comments)
                    .Where(x => x.Author.Contains(author))
                    .ToListAsync();
        }

        public async Task<List<PostEntity>> GetAllWithCommentsAsync(int likes)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                    .Include(p => p.Comments)
                    .Where(x => x.Comments != null && x.Comments.Any())
                    .ToListAsync();
        }

        public async Task<List<PostEntity>> GetAllByLikesAsync(int likes)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                    .Include(p => p.Comments)
                    .Where(x => x.Likes >= likes)
                    .ToListAsync();
        }

        public async Task<PostEntity> GetByIdAsync(Guid id)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(PostEntity postEntity)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Posts.Update(postEntity);

            _ = await context.SaveChangesAsync();
        }
    }
}
