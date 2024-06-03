using Bans.Model;
using BanSach2.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart , int productId,int count)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //shoppingCart.ApplicationUserId = claim.Value;

            // Tạo đối tượng ShoppingCart mới mà không thiết lập giá trị cho cột identity
            /*  shoppingCart = new ShoppingCart
              {
                  ProductId = productId,
                  count= count,

                  ApplicationUserId = claim.Value,
                  // Thiết lập các thuộc tính khác nếu cần

              };
              _unitOfwork.ShoppingCart.Add(shoppingCart);
              _unitOfwork.Save();*/

            ShoppingCart cartObj = _unitOfwork.ShoppingCart.GetFistOrDefault(
                u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);
            if(cartObj== null)
            {
                // _unitOfwork.ShoppingCart.Add(shoppingCart);
                _unitOfwork.ShoppingCart.Add(new ShoppingCart
                {
                    ApplicationUserId = claim.Value,
                    ProductId = shoppingCart.ProductId,
                    count = shoppingCart.count
                    // Không gán giá trị cho cột định danh Id
                });
            }
            else
            {
                _unitOfwork.ShoppingCart.IncrementCount(cartObj,shoppingCart.count);

            }
            _unitOfwork.Save();
            return RedirectToAction(nameof(Index));


          /*  ShoppingCart cartobj = new()
            {
                //Product = _unitOfwork.Product.GetFistOrDefault(u => u.Id == id, includeProperties: "Category,CoverType"),
                count = 1,
                
            };
            if (cartobj.Product != null)
            {
                return View(cartobj);
            }
            else
            {
                return NotFound();
            }*/
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