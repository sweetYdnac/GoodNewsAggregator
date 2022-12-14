using by.Reba.Core.DataTransferObjects.User;

namespace by.Reba.WebAPI.Models.Responces.Users
{
    public class GetUsersGridResponseModel
    {
        public IEnumerable<UserGridDTO> Users { get; set; }
    }
}
