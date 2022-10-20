﻿using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.DataTransferObjects.Source;
using static by.Reba.Core.TreeExtensions;

namespace by.Reba.Application.Models.Article
{
    public class ArticleDetailsVM
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string PosterUrl { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Assessment { get; set; }
        public string CategoryTitle { get; set; }
        public string RatingTitle { get; set; }
        public SourceDTO Source { get; set; }
        public IEnumerable<ITree<CommentDTO>> Comments { get; set; }
        public bool isAdmin { get; set; }
    }
}
