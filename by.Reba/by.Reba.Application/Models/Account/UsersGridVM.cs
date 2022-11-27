using by.Reba.Core;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.Core.SortTypes;

namespace by.Reba.Application.Models.Account
{
    public class UsersGridVM
    {
        public IEnumerable<UserGridDTO> Users { get; set; } = Enumerable.Empty<UserGridDTO>();
        public string SearchString { get; set; }
        public UserSort SortOrder { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
