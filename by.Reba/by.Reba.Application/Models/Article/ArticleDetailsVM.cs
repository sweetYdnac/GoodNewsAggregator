using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.DataTransferObjects.Source;
using by.Reba.Core.Tree;

namespace by.Reba.Application.Models.Article
{
    public class ArticleDetailsVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Assessment { get; set; }
        public string CategoryTitle { get; set; }
        public string RatingTitle { get; set; }
        public string SourceUrl { get; set; }
        public SourceDTO Source { get; set; }
        public IEnumerable<ITree<CommentDTO>> Comments { get; set; } = Enumerable.Empty<ITree<CommentDTO>>();
        public string UserEmail { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsAuthenticated { get; set; } = false;
    }
}
