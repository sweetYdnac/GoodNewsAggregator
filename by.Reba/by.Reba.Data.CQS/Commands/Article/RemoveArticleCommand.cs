﻿using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class RemoveArticleCommand : IRequest
    {
        public T_Article Article { get; set; }
    }
}
