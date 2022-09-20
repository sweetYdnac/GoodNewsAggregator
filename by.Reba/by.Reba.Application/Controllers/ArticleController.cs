﻿using by.Reba.Application.Models;
using by.Reba.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace by.Reba.Application.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetByPage(1, 9);
            var model = new HomePageVM()
            {
                Articles = articles
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}