using by.Reba.Data.Abstractions.Repositories;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.Repositories
{
    public sealed class CommentRepository : Repository<T_Comment>, ICommentRepository
    {
        public CommentRepository(RebaDbContext db) 
            : base(db)
        {
        }

        public async Task<T_Comment?> GetWithInnerTreeByIdAsync(Guid id)
        {
            var result = await DbSet.FromSqlRaw(
                @"WITH tree (id, content, assessment, creationTime, articleId, authorId, parentCommentId, below) AS (
                        SELECT id, content, assessment, creationTime, articleId, authorId, parentCommentId, 0
                        FROM dbo.Comments
                        WHERE Comments.Id = {0}
                        UNION ALL
                        SELECT c.id, c.content, c.assessment, c.creationTime, c.articleId, c.authorId, c.parentCommentId, t.below + 1
                        FROM dbo.Comments c
                        INNER JOIN tree t
                            ON t.Id = c.ParentCommentId
                     )
                     SELECT * FROM tree", id)
                .ToListAsync();

            var root = result.FirstOrDefault();
            await LoadAuthors(root);

            return root;
        }

        private async Task LoadAuthors(T_Comment rootComment)
        {
            await LoadAuthor(rootComment);

            foreach (var innerComment in rootComment.InnerComments)
            {
                await LoadAuthor(innerComment);
            }
        }

        private async Task<T_Comment> LoadAuthor(T_Comment comment)
        {
            await Database.Entry(comment)
                .Reference(c => c.Author)
                .LoadAsync();

            return comment;
        }
    }
}
