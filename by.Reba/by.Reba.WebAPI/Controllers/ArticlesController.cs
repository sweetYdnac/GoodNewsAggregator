using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using Microsoft.AspNetCore.Mvc;

namespace by.Reba.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with articles
    /// </summary>
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private const int COUNT_PER_PAGE = 12;

        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IHistoryService _historyService;
        private readonly IPositivityService _positivityService;
        private readonly ISourceService _sourceService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ArticlesController(
            IArticleService articleService,
            ICategoryService categoryService,
            IHistoryService historyService,
            IPositivityService positivityService,
            IRoleService roleService,
            ISourceService sourceService,
            IUserService userService,
            IMapper mapper,
            IConfiguration configuration) =>

            (_articleService, _categoryService, _historyService, _positivityService, _roleService, _sourceService, _userService, _mapper, _configuration) =
            (articleService, categoryService, historyService, positivityService, roleService, sourceService, userService, mapper, configuration);

        /// <summary>
        /// Get Article from storage with specified id
        /// </summary>
        /// <param name="id">Id of article</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArticleDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetArticleById(Guid id)
        {
            var article = await _articleService.GetByIdAsync(id);

            return article is null 
                ? NotFound() 
                : Ok(article);
        }


        //[HttpGet]
        //public async Task<IActionResult> GetArticles([FromQuery] GetArticlesRequestModel request)
        //{
        //    if (request is null)
        //    {
        //        return BadRequest(new ErrorModel() { Message = "Request model is null"});
        //    }


        //}
    }
}
