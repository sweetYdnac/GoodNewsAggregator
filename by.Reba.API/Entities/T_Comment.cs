
using by.Reba.API.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.API.Entities
{
    public class T_Comment: IAssessable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Content { get; set; }

        public int? Likes { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationTime { get; set; }

        public T_Notification T_Notification { get; set; }

    }
}
