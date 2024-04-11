

using Bans.Model;
using Bans.Model.ViewModel;
using BanSach2.DataAcess;
using BanSach2.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;



namespace BanSach2MVC.Areas.Admin.Controllers
{
   [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ProductController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objCategoryList = _UnitOfWork.Product.GetAll();
            return View(objCategoryList);
        }
    
        public IActionResult Upsert(int? id)
        {
           /* Product product = new Product();
            //convert list category thanh list item
            IEnumerable<SelectListItem> CategoryList = _UnitOfWork.Category.GetAll().Select(
                u=> new SelectListItem()
                {
                    //tat ca property o trong selectlistitem co the tuy chon 
                    Text=u.Name,
                    //truyen value sang string 
                    Value=u.ID.ToString(),

                     
                }
                                );
            IEnumerable <SelectListItem> CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(
                u => new SelectListItem()
                {
                    //tat ca property o trong selectlistitem co the tuy chon 
                    Text = u.Name,
                    //truyen value sang string 
                    Value = u.ID.ToString(),


                }
                                );*/
           ProductVM productVM = new ProductVM();
            productVM.product = new Product();
            productVM.CategoryList = _UnitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value=u.ID.ToString(),
                });
            productVM.CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(
               u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.ID.ToString(),
               });


            if (id == null || id == 0)
            {
                //create product
                /*ViewBag.CategoryList = CategoryList;
                ViewData["CoverTypeList"] = CoverTypeList;*/
                return View(productVM);

            }
            else
            {
                //update

            }
           
           
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
          /*  if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The name must not same DisplayOrder");
            }*/
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Update(obj);
                _UnitOfWork.Save();
                TempData["Sucess"] = "CoverType Update Successful";
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
            var categoryfromDB = _UnitOfWork.CoverType.GetFistOrDefault(u => u.ID == id);
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
            var obj = _UnitOfWork.CoverType.GetFistOrDefault(u => u.ID == id);
            if (obj.Name == null)
            {
                return NotFound();
            }
            else
            {
                _UnitOfWork.CoverType.Remove(obj);
                _UnitOfWork.Save();
                TempData["Sucess"] = "CoverType Delete Successful";
                return RedirectToAction("index");
            }

            return View(obj);
        }
    }
}
