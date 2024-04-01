﻿

using Bans.Model;
using BanSach2.DataAcess;
using BanSach2.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;



namespace BanSachMVC_Unica.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _UnitOfWork.Category.GetAll();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The name must not same DisplayOrder");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.Category.Save();
                TempData["Sucess"] = "Category Create Successful";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
           // var categoryfromDB = _db.Categories.Find(id);
             var categoryfromDBFirst = _UnitOfWork.Category.GetFistOrDefault(u=>u.ID == id);
            //  var categoryfromDBSingle = _db.Categories.SingleOrDefault(u=>u.ID == id);
            if (categoryfromDBFirst == null)
            {
                return NotFound();
            }
            return View(categoryfromDBFirst);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The name must not same DisplayOrder");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(obj);
                _UnitOfWork.Save();
                TempData["Sucess"] = "Category Update Successful";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryfromDB = _UnitOfWork.Category.GetFistOrDefault(u => u.ID == id);
            /*  var categoryfromDBFirst = _db.Categories.FirstOrDefault(u=>u.ID == id);
              var categoryfromDBSingle = _db.Categories.SingleOrDefault(u=>u.ID == id);*/
            if (categoryfromDB == null)
            {
                return NotFound();
            }
            return View(categoryfromDB);
        }
        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _UnitOfWork.Category.GetFistOrDefault(u => u.ID == id);
            if (obj.Name == null)
            {
                return NotFound();
            }
            else
            {
                _UnitOfWork.Category.Remove(obj);
                _UnitOfWork.Save();
                TempData["Sucess"] = "Category Delete Successful";
                return RedirectToAction("index");
            }

            return View(obj);
        }
    }
}
