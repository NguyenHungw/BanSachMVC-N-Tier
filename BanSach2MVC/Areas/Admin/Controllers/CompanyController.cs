using Bans.Model;
using Bans.Model.ViewModel;
using BanSach2.DataAcess;
using BanSach2.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Collections.Generic;



namespace BanSach2MVC.Areas.Admin.Controllers
{
   [Area("Admin")]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        public CompanyController(IUnitOfWork UnitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            
            return View();
        }
    
        public IActionResult Upsert(int? id)
        {
          
           Company company = new Company();


            if (id == null || id == 0)
            {

                //create product
                /*ViewBag.CategoryList = CategoryList;
                ViewData["CoverTypeList"] = CoverTypeList;*/
                return View(company);

            }
            else
            {
                //update product



                company = _UnitOfWork.Company.GetFistOrDefault(u=>u.Id==id);

            }

          

            return View(company);
        }
      /*  [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj,IFormFile file)
        {
          *//*  if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The name must not same DisplayOrder");
            }*//*
            if (ModelState.IsValid)
            {
                //upload images
                string wwwrootpath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string filename= Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootpath, @"images\products\");
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
                    obj.product.ImageURL = @"images\products\"+filename+extension;

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
        public IActionResult Deletecf(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryfromDB = _UnitOfWork.CoverType.GetFistOrDefault(u => u.ID == id);
            *//*  var categoryfromDBFirst = _db.Categories.FirstOrDefault(u=>u.ID == id);
              var categoryfromDBSingle = _db.Categories.SingleOrDefault(u=>u.ID == id);*//*
            if (categoryfromDB == null)
            {
                return NotFound();
            }
            return View(categoryfromDB);
        }*/
       
        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _UnitOfWork.Company.GetAll(includeProperties: "Category,CoverType");

          /*  var productList1 = _UnitOfWork.Product.GetAll( includeProperties: "Category");
            var productList2 = _UnitOfWork.Product.GetAll(includeProperties: "CoverType");

            var result = productList1.Concat(productList2).ToList();*/


            return Json(new {data = companyList });
        }

        [HttpDelete, ActionName("Delete")]

       // [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _UnitOfWork.Company.GetFistOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _UnitOfWork.Company.Remove(obj);
                _UnitOfWork.Save();
                return Json(new {success = true,message=""});
            }

            return View(obj);
        }

        #endregion
    }
}
