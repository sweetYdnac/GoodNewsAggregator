using by.Reba.Core;
using by.Reba.Core.DataTransferObjects;
using by.Reba.DataBase.Entities;
using by.Reba.DataBase.Interfaces;

namespace by.Reba.Business.Helpers
{
    public static class EntityExtensions
    {
        public static List<PatchModel> CreateRatePatchList<T>(this T entity, RateEntityDTO dto, T_User author) where T : IAssessable
        {
            if (author is null)
            {
                throw new ArgumentNullException($"Author is null", nameof(dto.AuthorId));
            }

            var UsersWithPositiveAssessment = entity.UsersWithPositiveAssessment;
            var UsersWithNegativeAssessment = entity.UsersWithNegativeAssessment;

            var patchList = new List<PatchModel>()
            {
                new PatchModel()
                {
                    PropertyName = nameof(UsersWithPositiveAssessment),
                    PropertyValue = UsersWithPositiveAssessment,
                },
                new PatchModel()
                {
                    PropertyName = nameof(UsersWithNegativeAssessment),
                    PropertyValue = UsersWithNegativeAssessment,
                }
            };

            var posUser = UsersWithPositiveAssessment.FirstOrDefault(u => u.Id.Equals(dto.AuthorId));
            var negUser = UsersWithNegativeAssessment.FirstOrDefault(u => u.Id.Equals(dto.AuthorId));

            Action t = (dto.IsLike, posUser, negUser) switch
            {
                (true, not null, _) => () => UsersWithPositiveAssessment.Remove(posUser),
                (true, _, not null) => () =>
                {
                    UsersWithNegativeAssessment.Remove(negUser);
                    UsersWithPositiveAssessment.Add(negUser);
                }
                ,
                (true, null, null) => () => UsersWithPositiveAssessment.Add(author),
                (false, _, not null) => () => UsersWithNegativeAssessment.Remove(negUser),
                (false, not null, _) => () =>
                {
                    UsersWithPositiveAssessment.Remove(posUser);
                    UsersWithNegativeAssessment.Add(posUser);
                },
                (false, null, null) => () => UsersWithNegativeAssessment.Add(author)
            };

           t.Invoke();

            return patchList;
        }
    }
}
