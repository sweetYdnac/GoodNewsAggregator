using AutoMapper;
using by.Reba.Application.Models.Comment;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;

namespace by.Reba.Application.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CommentController(
            ICommentService commentService,
            IUserService userService,
            IMapper mapper) =>

            (_commentService, _userService, _mapper) =
            (commentService, userService, mapper);

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentVM model)
        {
            // todo: Сохранение в бд переносов текста
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<CreateCommentDTO>(model);
                    dto.AuthorId = await _userService.GetIdByEmailAsync(HttpContext.User.Identity.Name);

                    var result = await _commentService.CreateAsync(dto);
                }

                return RedirectToAction("Details", "Article", new { id = model.ArticleId }, "#comments");
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Rate(RateCommentVM model)
        {
            try
            {
                var dto = _mapper.Map<RateEntityDTO>(model);
                dto.AuthorId = await _userService.GetIdByEmailAsync(HttpContext.User.Identity.Name);

                var result = await _commentService.RateAsync(dto);
                return RedirectToAction("Details", "Article", new { id = model.ArticleId });
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
