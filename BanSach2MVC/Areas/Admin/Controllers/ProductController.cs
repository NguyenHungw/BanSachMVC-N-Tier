

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
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork UnitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objCategoryList = _UnitOfWork.Product.GetAll();
            return View(objCategoryList);
        }
    
        public IActionResult Upsert(int? id)
        {
          
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
                //update product



                productVM.product = _UnitOfWork.Product.GetFistOrDefault(u=>u.Id==id);

            }

          

            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile file)
        {
          /*  if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The name must not same DisplayOrder");
            }*/
            if (ModelState.IsValid)
            {
                //upload images
                string wwwrootpath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string filename= Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootpath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    if(obj.product.ImageURL != null)
                    {
                        var oldImagePath = Path.Combine(wwwrootpath, obj.product.ImageURL.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                    }
                    obj.product.ImageURL = @"images\products"+filename+extension;

                }
                if (obj.product.Id == 0)
                {
                    _UnitOfWork.Product.Add(obj.product);

                }
                else
                {
                    _UnitOfWork.Product.Update(obj.product);
                    _UnitOfWork.Product.Remove(obj.product);
                }
                //_UnitOfWork.Product.Add(obj.product);
                _UnitOfWork.Save();
                TempData["Sucess"] = "product create Successful";
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

        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _UnitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

          /*  var productList1 = _UnitOfWork.Product.GetAll( includeProperties: "Category");
            var productList2 = _UnitOfWork.Product.GetAll(includeProperties: "CoverType");

            var result = productList1.Concat(productList2).ToList();*/


            return Json(new {data = productList });
        }
        #endregion
    }
}
