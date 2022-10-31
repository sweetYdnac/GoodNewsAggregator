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

            if (entity is null)
            {
                throw new ArgumentException(nameof(dto));
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


            if (author is null)
            {
                throw new ArgumentException(nameof(dto.AuthorId));
            }

            if (dto.IsLike)
            {
                if (posUser is not null)
                {
                    UsersWithPositiveAssessment.Remove(posUser);
                }
                else if (negUser is not null)
                {
                    UsersWithNegativeAssessment.Remove(negUser);
                    UsersWithPositiveAssessment.Add(negUser);
                }
                else
                {
                    UsersWithPositiveAssessment.Add(author);
                }
            }
            else
            {
                if (negUser is not null)
                {
                    UsersWithNegativeAssessment.Remove(negUser);
                }
                else if (posUser is not null)
                {
                    UsersWithPositiveAssessment.Remove(posUser);
                    UsersWithNegativeAssessment.Add(posUser);
                }
                else
                {
                    UsersWithNegativeAssessment.Add(author);
                }
            }

            return patchList;
        }
    }
}
