using AutoMapper;
using by.Reba.Application.Models.Comment;
using by.Reba.Application.Models.Source;
using by.Reba.Business.ServicesImplementations;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.DataTransferObjects.Source;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                return RedirectToAction("Details", "Article", new { id = model.ArticleId }, $"#comment{model.Id}");
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditCommentVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<EditCommentDTO>(model);
                    var result = await _commentService.UpdateAsync(model.Id, dto);

                    return RedirectToAction("Details", "Article", new { id = model.ArticleId }, $"#comment{model.Id}");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
