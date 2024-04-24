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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
         
            if (ModelState.IsValid)
            {
                //upload images
              
                if (obj.Id == 0)
                {
                    
                    _UnitOfWork.Company.Add(obj);

                }
                else
                {
                    _UnitOfWork.Company.Update(obj);
                    
                   
                   
                }
                //_UnitOfWork.Product.Add(obj.product);
                _UnitOfWork.Save();
                TempData["Sucess"] = "company create Successful";
                return RedirectToAction("index");
            }
            return View(obj);
        }
      
        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _UnitOfWork.Company.GetAll();

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
