using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;

namespace SampleMVC.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleBLL _articleBLL;
        private readonly ICategoryBLL _categoryBLL;
        public ArticleController(IArticleBLL articleBLL, ICategoryBLL categoryBLL)
        {
            _articleBLL = articleBLL;
            _categoryBLL = categoryBLL;
        }
        public IActionResult Index(int pageNumber = 1, int pageSize = 7, string search = "", string act = "", int? categoryId = null)
        {
            if (TempData["message"] != null)
            {
                ViewData["message"] = TempData["message"];
            }

            ViewData["search"] = search;
            ViewData["selectedCategoryId"] = categoryId;

            // Mendapatkan daftar kategori
            var categories = _categoryBLL.GetAll();  // Sesuaikan dengan metode yang Anda miliki
            ViewBag.Categories = categories;

            var totalItems = categoryId.HasValue
                ? _articleBLL.GetCountArticleByCategory(categoryId.Value, search)
                : _articleBLL.GetCountArticle(search);

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (act == "next" && pageNumber < totalPages)
            {
                pageNumber += 1;
            }
            else if (act == "prev" && pageNumber > 1)
            {
                pageNumber -= 1;
            }

            ViewData["pageNumber"] = pageNumber;
            ViewData["pageSize"] = pageSize;
            ViewData["totalPages"] = totalPages;

            IEnumerable<ArticleDTO> models;
            // int categoryId, int pageNumber, int pageSize, string search
            if (categoryId.HasValue)
            {
                models = _articleBLL.GetArticleByCategoryPaging(categoryId.Value, pageNumber, pageSize, search);
            }
            else
            {
                models = _articleBLL.GetWithPaging(pageNumber, pageSize, search);
            }

            return View(models);
        }
        // GET: ArticleController
        //public IActionResult Index()
        //{
        //    var model = _articleBLL.GetArticleWithCategory();

        //    return View(model);
        //}

        // GET: ArticleController/Details/5
        public IActionResult Details(int id)
        {
            var model = _articleBLL.GetArticleById(id);
            return View(model);
        }

        // GET: ArticleController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArticleController/Create
        //[HttpPost]
        //public IActionResult Create(ArticleCreateDTO articleCreate)
        //{
        //    try
        //    {
        //        _articleBLL.Insert(articleCreate);
        //        TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Category Success !</div>";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["message"] = @"<div class='alert alert-danger'><strong>Failed!</strong>" + ex.Message + "</div>";
        //    }
        //    return RedirectToAction("Index");

        //}

        [HttpPost]
        public IActionResult Create(ArticleCreateDTO articleCreate, IFormFile imageArticle)
        {
            try
            {
                if (imageArticle != null && imageArticle.Length > 0)
                {

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageArticle.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "picts", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageArticle.CopyTo(stream);
                    }

                    articleCreate.Pic = fileName;
                }

                _articleBLL.Insert(articleCreate);

                TempData["message"] = "<div class='alert alert-success'><strong>Success!</strong> Add Data Category Success !</div>";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            }

            return RedirectToAction("Index");
        }



        // GET: ArticleController/Edit/5
        public IActionResult Edit(int id)
        {
            var model = _articleBLL.GetArticleById(id);
            if (model == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Category Not Found !</div>";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // POST: ArticleController/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, ArticleUpdateDTO articleEdit)
        {
            try
            {
                _articleBLL.Update(articleEdit);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Edit Data Category Success !</div>";
            }
            catch
            {
                ViewData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Failed to edit data category !</div>";
                return View(articleEdit);
            }
            return RedirectToAction("Index");
        }

        // GET: ArticleController/Delete/5
        public IActionResult Delete(int id)
        {
            var model = _articleBLL.GetArticleById(id);
            if (model == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Category Not Found !</div>";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // POST: ArticleController/Delete/5
        [HttpPost]
        public IActionResult Delete(int id, ArticleDTO article)
        {
            try
            {
                _articleBLL.Delete(id);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Delete Data Category Success !</div>";
            }
            catch
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Failed to delete data category !</div>";
            }
            return RedirectToAction("Index");
        }
        public IActionResult GetArticleByCategory(int categoryId)
        {
            try
            {
                _articleBLL.GetArticleByCategory(categoryId);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Get Article By Category Success !</div>";
            }
            catch
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Failed to get article by category !</div>";
            }
            return RedirectToAction("Index");
        }
        public IActionResult GetArticleById(int id)
        {
            try
            {
                _articleBLL.GetArticleById(id);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Get Article By Id Success !</div>";
            }
            catch
            {
                TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Failed to get article by id !</div>";
            }
            return RedirectToAction("Index");
        }


    }
}
