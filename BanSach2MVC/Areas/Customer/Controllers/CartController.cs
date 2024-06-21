using Bans.Model;
using Bans.Model.ViewModel;
using BanSach2.DataAcess.Repository.IRepository;
using BanSach2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Drawing.Printing;
using System.Security.Claims;
using System.Xml.Linq;

namespace BanSach2MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    [BindProperties]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderTotl { get; set; }

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
                model.orderHeader = new OrderHeader();

            };
            foreach(var cart in model.ListCart)
            {
                cart.Price = GetPriceBaseOnQuantity(cart.count,
                     cart.Product.Price100);
                model.CartTotal += (cart.Product.Price100 * cart.count);
                model.orderHeader.OrderTotal += (cart.Price * cart.count);
                
            }

            return View(model); 
        }
        public IActionResult Summary()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM model = new ShoppingCartVM();
            {
                //  ienumerable<bans.model.shoppingcart> listcart = _unitofwork.shoppingcart.getall();
                /* ienumerable<bans.model.shoppingcart> listcart = _unitofwork.shoppingcart.getall(u=> u.applicationuserid == claim.value,
                     includeproperties:"product");*/

                //  ienumerable<bans.model.shoppingcart> listcart = _unitofwork.shoppingcart.getall();
                model.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                    includeProperties: "Product");
                model.orderHeader= new OrderHeader();

            };
            model.orderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFistOrDefault(
                u=>u.Id==claim.Value
                );
            model.orderHeader.Name= model.orderHeader.ApplicationUser.Name;
            model.orderHeader.PhoneNumber = model.orderHeader.ApplicationUser.PhoneNumber;

            model.orderHeader.City = model.orderHeader.ApplicationUser.City;

            model.orderHeader.State = model.orderHeader.ApplicationUser.State;
            model.orderHeader.PostalCode = model.orderHeader.ApplicationUser.PostalCode;


            foreach (var cart in model.ListCart)
            {
                cart.Price = GetPriceBaseOnQuantity(cart.count,
                     cart.Product.Price100);
                model.CartTotal += (cart.Product.Price100 * cart.count);
                model.orderHeader.OrderTotal += (cart.Price * cart.count);

            }

            return View(model);
            

        }
        [ActionName ("Summary")]
        [HttpPost]
        public IActionResult SummaryPOST()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

            var applicationUser = _unitOfWork.ApplicationUser.GetFistOrDefault(u => u.Id == claim.Value);

            ShoppingCartVM model = new ShoppingCartVM();
            {
              
              
                model.orderHeader = new OrderHeader();
                model.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product");
            };
          
            model.orderHeader.PaymentStatus = SD.PaymentStatusPending;
            model.orderHeader.OrderStatus = SD.StatusPending;
            model.orderHeader.OrderDate= DateTime.Now;
            model.orderHeader.ApplicationUserId= claim.Value;
            model.orderHeader.City = "Default"; // Ensure City is set to a default value or obtained from user input
            model.orderHeader.StreetAdress = "Default"; // Ensure StreetAddress is set to a default value or obtained from user input
            model.orderHeader.PhoneNumber = "Default"; // Ensure PhoneNumber is set to a default value or obtained from user input
            model.orderHeader.Name = applicationUser.Name; // Ensure Name is set to a default value or obtained from user input
            model.orderHeader.PostalCode = "Default"; // Ensure PostalCode is set to a default value or obtained from user input
            model.orderHeader.State = "Default";
            foreach (var cart in model.ListCart)
            {
                cart.Price = GetPriceBaseOnQuantity(cart.count,
                     cart.Product.Price100);
                model.CartTotal += (cart.Product.Price100 * cart.count);
                model.orderHeader.OrderTotal += (cart.Price * cart.count);

            }

            _unitOfWork.OrderHeader.Add(model.orderHeader);
            _unitOfWork.Save();
            foreach (var cart in model.ListCart)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    Name = cart.Product.Name,
                    ProductID = cart.ProductId,
                    OrderID = model.orderHeader.Id,
                    Price = cart.Price,
                    Count = cart.count,
                };
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Save();
            }
            _unitOfWork.ShoppingCart.RemoveRange(model.ListCart);
            _unitOfWork.Save();
            return RedirectToAction("Index","Home");



        }
        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFistOrDefault(u=>u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart,1);
            _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFistOrDefault(u => u.Id == cartId);
           if(cart.count <= 1)
            {
                Remove(cartId);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFistOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBaseOnQuantity(int quantity,double price100)
        {
            /*  if(quantity <= 50)
              {
                  return price;
              }
              else
              {
                  if (quantity <= 100)
                  {
                      return price50;
                  }
                  return price100;
              }*/
            
          if(quantity < 1)
            {
            return 0;
            }
            else
            {
                return  quantity * price100;
                
            }
          
        }
    }
}
