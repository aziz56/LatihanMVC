﻿using MyWebFormApp.BO;
using System.Collections.Generic;

namespace MyWebFormApp.DAL.Interfaces
{
    public interface IArticleDAL : ICrud<Article>
    {
        IEnumerable<Article> GetArticleWithCategory();
        //aaa
        IEnumerable<Article> GetArticleByCategory(int categoryId);
        int InsertWithIdentity(Article article);

        void InsertArticleWithCategory(Article article);

        IEnumerable<Article> GetWithPaging(int pageNumber, int pageSize, string name);
        int GetCountArticle(string name);
        IEnumerable<Article> GetArticleByCategoryPaging(int categoryId, int pageNumber, int pageSize, string search);
        int GetCountArticleByCategory(int categoryId, string search);
    }
}
