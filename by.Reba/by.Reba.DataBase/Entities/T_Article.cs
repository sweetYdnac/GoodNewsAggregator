using by.Reba.DataBase.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace by.Reba.DataBase.Entities
{
    public class T_Article : IBaseEntity, IAssessable
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? HtmlContent { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string PosterUrl { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublicationDate { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string SourceUrl { get; set; }


        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public T_Category Category { get; set; }

        [ForeignKey(nameof(Positivity))]
        public Guid? PositivityId { get; set; }
        public T_Positivity? Positivity { get; set; }

        [ForeignKey(nameof(Source))]
        public Guid SourceId { get; set; }
        public T_Source Source { get; set; }

        public ICollection<T_Comment> Comments { get; set; }
        public ICollection<T_History> History { get; set; }
        public ICollection<T_User> UsersWithPositiveAssessment { get; set; }
        public ICollection<T_User> UsersWithNegativeAssessment { get; set; }
    }
}