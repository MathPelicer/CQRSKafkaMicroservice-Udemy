using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _databaseContextFactory;

        public CommentRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task CreateAsync(CommentEntity commentEntity)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Comments.Add(commentEntity);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var comment = await GetByIdAsync(id);

            if (comment == null) return;

            context.Comments.Remove(comment);
            _ = await context.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid id)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Comments
                    .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(CommentEntity commentEntity)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Comments.Update(commentEntity);

            _ = await context.SaveChangesAsync();
        }
    }
}
