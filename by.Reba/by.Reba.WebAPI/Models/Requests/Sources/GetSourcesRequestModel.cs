using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Sources
{
    public class GetSourcesRequestModel
    {
        public string SearchString { get; set; } = string.Empty;

        public int Page { get; set; } = 1;

        public int CountPerPage { get; set; } = 15;
    }
}
