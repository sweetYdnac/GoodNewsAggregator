﻿using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.DataTransferObjects.Source;
using by.Reba.Core.Tree;
using static by.Reba.Core.Tree.TreeExtensions;

namespace by.Reba.Core.DataTransferObjects.Article
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string SourceUrl { get; set; }
        public string PosterUrl { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Assessment { get; set; }
        public string CategoryTitle { get; set; }
        public string RatingTitle { get; set; }
        public SourceDTO Source { get; set; }
        public IEnumerable<ITree<CommentDTO>>? CommentTrees { get; set; }
    }
}
