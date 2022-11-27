using System.ComponentModel.DataAnnotations.Schema;
using by.Reba.DataBase.Interfaces;

namespace by.Reba.DataBase.Entities
{
    public class T_Preference : IBaseEntity
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public T_User User { get; set; }

        [ForeignKey(nameof(MinPositivityRating))]
        public Guid PositivityRatingId { get; set; }
        public T_Positivity MinPositivityRating { get; set; }

        public ICollection<T_Category> Categories { get; set; }
    }
}
