using by.Reba.API.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace by.Reba.API.Entities
{
    public class T_Article: IAssessable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(512)]
        public string Description { get; set; }

        [Required]
        [MaxLength(512)]
        [DataType(DataType.Url)]
        public string OriginUrl { get; set; }

        [Required]
        [MaxLength(512)]
        [DataType(DataType.ImageUrl)]
        public string PosterUrl { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreationTime { get; set; }

        public int? Likes { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public T_Category Category { get; set; }

        [ForeignKey(nameof(Rating))]
        public int RatingId { get; set; }

        public T_PositivityRating Rating { get; set; }

        public ICollection<T_User> UserBookmarks { get; set; }

        public ICollection<T_User> UserHistory { get; set; }

        public ICollection<T_Comment> Comments { get; set; }     
    }
}
