namespace by.Reba.WebAPI.Models.Requests.Positivity
{
    public class PatchPositivityRequestModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public float Value { get; set; }
    }
}
