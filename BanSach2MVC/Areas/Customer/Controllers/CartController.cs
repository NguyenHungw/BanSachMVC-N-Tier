using Bans.Model;
using Bans.Model.ViewModel;
using BanSach2.DataAcess.Repository.IRepository;
using BanSach2.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Stripe.Checkout;
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
        [HttpGet]
        public IActionResult Success(string session_id)
        {
            // Xử lý session_id nếu cần
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            // Lấy OrderHeader từ cơ sở dữ liệu
            var orderHeader = _unitOfWork.OrderHeader.GetFistOrDefault(o => o.ApplicationUserId == userId && o.SessionID == session_id);

            if (orderHeader == null)
            {
                // Xử lý khi không tìm thấy đơn hàng
                return NotFound();
            }

            orderHeader.OrderStatus = SD.StatusApproved;
            _unitOfWork.Save();

            // Xóa giỏ hàng của người dùng
            var userCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(userCart);
            _unitOfWork.Save();

            // Truyền OrderHeader tới view
            return View(orderHeader);
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
        [ActionName("Summary")]
        [HttpPost]
        public IActionResult SummaryPOST()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

            var applicationUser = _unitOfWork.ApplicationUser.GetFistOrDefault(u => u.Id == claim.Value);

            ShoppingCartVM model = new ShoppingCartVM();
            model.orderHeader = new OrderHeader();
            model.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product");

            model.orderHeader.PaymentStatus = SD.PaymentStatusPending;
            model.orderHeader.OrderStatus = SD.StatusPending;
            model.orderHeader.OrderDate = DateTime.Now;
            model.orderHeader.ApplicationUserId = claim.Value;
            model.orderHeader.City = applicationUser.City ?? "Default";
            model.orderHeader.StreetAdress = applicationUser.StreetAddress ?? "Default";
            model.orderHeader.PhoneNumber = applicationUser.PhoneNumber ?? "Default";
            model.orderHeader.Name = applicationUser.Name ?? "Default";
            model.orderHeader.PostalCode = applicationUser.PostalCode ?? "Default";
            model.orderHeader.State = applicationUser.State ?? "Default";

            foreach (var cart in model.ListCart)
            {
                cart.Price = GetPriceBaseOnQuantity(cart.count, cart.Product.Price100);
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

            // PAYMENT SETTINGS
            var domain = "https://localhost:7273";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                // SuccessUrl = domain + "/success?session_id={CHECKOUT_SESSION_ID}",
                // SuccessUrl = domain + "/customer/cart/success?session_id={CHECKOUT_SESSION_ID}",
                SuccessUrl = domain + "/customer/cart/Success",

                CancelUrl = domain + "/customer/cart/Index",
            };

            foreach (var item in model.ListCart)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
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
