

using Bans.Model;
using BanSach2.DataAcess;
using BanSach2.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;



namespace BanSach2MVC.Areas.Admin.Controllers
{
   [Area("Admin")]

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CoverTypeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCategoryList = _UnitOfWork.CoverType.GetAll();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
   
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Add(obj);
                _UnitOfWork.Save();
                TempData["Sucess"] = "CoverType Create Successful";
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
            var categoryfromDBFirst = _UnitOfWork.CoverType.GetFistOrDefault(u => u.ID == id);
            //  var categoryfromDBSingle = _db.Categories.SingleOrDefault(u=>u.ID == id);
            if (categoryfromDBFirst == null)
            {
                return NotFound();
            }
            return View(categoryfromDBFirst);
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
