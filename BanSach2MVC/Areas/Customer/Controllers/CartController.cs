using Bans.Model;
using Bans.Model.ViewModel;
using BanSach2.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace BanSach2MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimIdentity =(ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM  model = new ShoppingCartVM();
            {
                //  IEnumerable<Bans.Model.ShoppingCart> ListCart = _unitOfWork.ShoppingCart.GetAll();
                /* IEnumerable<Bans.Model.ShoppingCart> ListCart = _unitOfWork.ShoppingCart.GetAll(u=> u.ApplicationUserId == claim.Value,
                     includeProperties:"Product");*/

                //  IEnumerable<Bans.Model.ShoppingCart> ListCart = _unitOfWork.ShoppingCart.GetAll();
                model.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                    includeProperties: "Product");
            };

            return View(model); 
        }
    }
}
