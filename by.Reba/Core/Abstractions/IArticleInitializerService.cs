﻿namespace by.Reba.Core.Abstractions
{
    public interface IArticleInitializerService
    {
        Task<int> CreateArticlesFromExternalSourcesAsync();
        Task<int> AddTextToArticlesAsync();
        Task<int> AddRatingToArticlesAsync();
    }
}
