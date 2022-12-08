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

            var usersWithPositiveAssessment = entity.UsersWithPositiveAssessment;
            var usersWithNegativeAssessment = entity.UsersWithNegativeAssessment;

            var patchList = new List<PatchModel>()
            {
                new PatchModel()
                {
                    PropertyName = nameof(usersWithPositiveAssessment),
                    PropertyValue = usersWithPositiveAssessment,
                },
                new PatchModel()
                {
                    PropertyName = nameof(usersWithNegativeAssessment),
                    PropertyValue = usersWithNegativeAssessment,
                }
            };

            var posUser = usersWithPositiveAssessment.FirstOrDefault(u => u.Id.Equals(dto.AuthorId));
            var negUser = usersWithNegativeAssessment.FirstOrDefault(u => u.Id.Equals(dto.AuthorId));

            Action assessmentEntity = (dto.IsLike, posUser, negUser) switch
            {
                (true, not null, _) => () => usersWithPositiveAssessment.Remove(posUser),
                (true, _, not null) => () =>
                {
                    usersWithNegativeAssessment.Remove(negUser);
                    usersWithPositiveAssessment.Add(negUser);
                }
                ,
                (true, null, null) => () => usersWithPositiveAssessment.Add(author),
                (false, _, not null) => () => usersWithNegativeAssessment.Remove(negUser),
                (false, not null, _) => () =>
                {
                    usersWithPositiveAssessment.Remove(posUser);
                    usersWithNegativeAssessment.Add(posUser);
                },
                (false, null, null) => () => usersWithNegativeAssessment.Add(author)
            };

           assessmentEntity.Invoke();
           return patchList;
        }
    }
}
