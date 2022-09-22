using by.Reba.DataBase.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace by.Reba.DataBase.Entities
{
    public class T_Article : IBaseEntity, IAssessable
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(512)]
        public string Description { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [MaxLength(512)]
        [DataType(DataType.ImageUrl)]
        public string PosterUrl { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublicationDate { get; set; }

        public int? Assessment { get; set; }

        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public T_Category Category { get; set; }

        [ForeignKey(nameof(Rating))]
        public Guid RatingId { get; set; }
        public T_PositivityRating Rating { get; set; }

        [ForeignKey(nameof(Source))]
        public Guid SourceId { get; set; }
        public T_Source Source { get; set; }

        public ICollection<T_User> UserBookmarks { get; set; }

        public ICollection<T_User> UserHistory { get; set; }

        public ICollection<T_Comment> Comments { get; set; }
    }
}