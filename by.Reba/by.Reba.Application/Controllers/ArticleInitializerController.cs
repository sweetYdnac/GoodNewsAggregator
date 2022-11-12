using Microsoft.AspNetCore.Mvc;

namespace by.Reba.Application.Controllers
{
    public class ArticleInitializerController : Controller
    {


        public ArticleInitializerController()
        {

        }

        [HttpPost]
        public async Task<IActionResult> AddArticles()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
