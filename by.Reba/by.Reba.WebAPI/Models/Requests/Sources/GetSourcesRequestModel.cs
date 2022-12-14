using by.Reba.WebAPI.Models.Requests.QueryStringParameters.Pagination;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Sources
{
    public class GetSourcesRequestModel : PaginationParameters
    {
        public string SearchString { get; set; } = string.Empty;
    }
}
