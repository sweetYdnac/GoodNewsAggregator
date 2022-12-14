using by.Reba.Core.SortTypes;
using by.Reba.WebAPI.Models.Requests.QueryStringParameters.Pagination;

namespace by.Reba.WebAPI.Models.Requests.Users
{
    public class GetUsersGridRequestModel : PaginationParameters
    {
        public string SearchString { get; set; } = string.Empty;
        public UserSort SortOrder { get; set; } = UserSort.Email;
    }
}
