using Bans.Model;
using BanSach2.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BanSach2MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfwork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfwork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable <Product> products = _unitOfwork.Product.GetAll(includeProperties:"Category");
            return View(products);
        }
        public IActionResult Details(int id)
        {
            ShoppingCart cartobj = new()
            {
                Product = _unitOfwork.Product.GetFistOrDefault(u => u.Id == id, includeProperties: "Category,CoverType"),
                count = 1,
                ProductId = id
            };
            if(cartobj.Product != null)
            {
                return View(cartobj);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}