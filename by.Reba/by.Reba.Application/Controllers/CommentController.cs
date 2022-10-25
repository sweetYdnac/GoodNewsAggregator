using AutoMapper;
using by.Reba.Application.Models.Account;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace by.Reba.Application.Controllers
{
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        public CommentController(
            IMapper mapper)
        {
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Guid articleId, string text)
        {
            return Ok();
        }
    }
}
