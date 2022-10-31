using by.Reba.DataBase.Entities;

namespace by.Reba.DataBase.Interfaces
{
    public interface IAssessable
    {
        public ICollection<T_User> UsersWithPositiveAssessment { get; set; }
        public ICollection<T_User> UsersWithNegativeAssessment { get; set; }
    }
}