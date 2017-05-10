using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YHRSys.Models;
using YHRSys.ViewModels;

namespace YHRSys.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext storeDB = new ApplicationDbContext();
        // 
        // GET: /ShoppingCart/ 
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel 
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
                QtyTotal = cart.GetCount()
            };
            // Return the view 
            return View(viewModel);
        }
        // 
        // GET: /Store/AddToCart/5 
        /*
        public ActionResult AddToCart(int id)
        {
            // Retrieve the variety from the database 
            var addedVariety = storeDB.Varieties
                .Single(v => v.varietyId == id);

            // Add it to the shopping cart 
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedVariety);

            // Go back to the main store page for more shopping 
            return RedirectToAction("Index");
        }*/

        // AJAX: /ShoppingCart/UpdateCartCount/5 
        [HttpPost]
        public ActionResult AddToCart(int id, int cartCount)
        {
            ShoppingCartRemoveViewModel results = null;
            try
            {
                // Get the cart 
                var cart = ShoppingCart.GetCart(this.HttpContext);

                // Get the name of the album to display confirmation 
                var addedVariety = storeDB.Varieties.Single(v => v.varietyId == id);

                // Update the cart count 
                //int itemCount = cart.UpdateCartCount(id, cartCount);
                int itemCount = cart.AddToCart(addedVariety);

                //Prepare messages
                string msg = "The quantity of " + Server.HtmlEncode(addedVariety.FullDescription) +
                        " has been refreshed in your shopping cart.";
                if (itemCount == 0) msg = Server.HtmlEncode(addedVariety.FullDescription) +
                        " has been removed from your shopping cart.";
                //
                // Display the confirmation message 
                results = new ShoppingCartRemoveViewModel
                {
                    Message = msg,
                    CartTotal = cart.GetTotal(),
                    QtyTotal = cart.GetCount(),
                    CartCount = cart.GetCount(),
                    ItemCount = itemCount,
                    DeleteId = id
                };
            }
            catch (Exception ex)
            {
                results = new ShoppingCartRemoveViewModel
                {
                    Message = "Error occurred or invalid input... " + ex.Message,
                    CartTotal = -1,
                    CartCount = -1,
                    QtyTotal = -1,
                    ItemCount = -1,
                    DeleteId = id
                };
            }
            return Json(results);
        }

        // AJAX: /ShoppingCart/UpdateCartCount/5 
        [HttpPost]
        public ActionResult UpdateCartCount(int id, int cartCount)
        {
            ShoppingCartRemoveViewModel results = null;
            try
            {
                // Get the cart 
                var cart = ShoppingCart.GetCart(this.HttpContext);

                // Get the name of the album to display confirmation 
                string varietyName = storeDB.Carts
                    .Single(item => item.RecordId == id).Variety.FullDescription;

                // Update the cart count 
                int itemCount = cart.UpdateCartCount(id, cartCount);

                //Prepare messages
                string msg = "The quantity of " + Server.HtmlEncode(varietyName) +
                        " has been refreshed in your shopping cart.";
                if (itemCount == 0) msg = Server.HtmlEncode(varietyName) +
                        " has been removed from your shopping cart.";
                //
                // Display the confirmation message 
                results = new ShoppingCartRemoveViewModel
                {
                    Message = msg,
                    CartTotal = cart.GetTotal(),
                    QtyTotal = cart.GetCount(),
                    CartCount = cart.GetCount(),
                    ItemCount = itemCount,
                    DeleteId = id
                };                
            }
            catch(Exception ex)
            {
                results = new ShoppingCartRemoveViewModel
                {
                    Message = "Error occurred or invalid input... " + ex.Message,
                    CartTotal = -1,
                    CartCount = -1,
                    QtyTotal = -1,
                    ItemCount = -1,
                    DeleteId = id
                };
            }
            return Json(results);
        }

        // 
        // AJAX: /ShoppingCart/RemoveFromCart/5 
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart 
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the variety to display confirmation 
            string varietyName = storeDB.Carts
                .Single(item => item.RecordId == id).Variety.FullDescription;

            // Remove from cart 
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message 
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(varietyName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                QtyTotal = cart.GetCount(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        // 
        // GET: /ShoppingCart/CartSummary 
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}