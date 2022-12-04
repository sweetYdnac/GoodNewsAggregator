namespace by.Reba.Core.DataTransferObjects.Comment
{
    public class CreateCommentDTO
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string Content { get; set; }
        public Guid AuthorId { get; set; }
    }
}
