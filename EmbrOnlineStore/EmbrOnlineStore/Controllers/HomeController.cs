using EmbrOnlineStore.Controllers.Utilities;
using EmbrOnlineStore.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmbrOnlineStore.Controllers
{
    public class HomeController : Controller
    {
        public ShopModel model;
        public ActionResult Index()
        {
            Session["model"] = TestDataGenerator.PopulateDummyModel();
            return View();
        }

        /// <summary>
        /// HttpPost interface for editing cart item quantity.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        [HttpPost]
        public void EditCartItemQty(string itemId, string quantity)
        {
            model = (ShopModel)Session["model"]; // get the model object
            Item currentItem = null;
            // For each item in the item catalog, check whether the item's ID matches the
            // input ID.
            foreach (var i in model.itemCatalog)
            {
                if (i.itemID == Int32.Parse(itemId)/*Parse Item ID, TODO: exception handling... */)
                {
                    currentItem = i; break; // there is a match - store the matching item and break out of the loop
                }
            }

            if (currentItem != null) // if we have found an item
            {
                model.shoppingCart = CheckoutFacilitator.EditQuantity(currentItem, Int32.Parse(quantity), model);
            }
            Session["model"] = model; // reassign model.
        }

        /// <summary>
        /// HttpPost interface for deleting an item from the cart
        /// </summary>
        /// <param name="itemId"></param>
        [HttpPost]
        public void DeleteItemFromCart(string itemId)
        {
            model = (ShopModel)Session["model"]; // get the model object
            Item currentItem = null;
            // For each item in the item catalog, check whether the item's ID matches the
            // input ID.
            foreach (var i in model.itemCatalog)
            {
                if (i.itemID == Int32.Parse(itemId)/*Parse Item ID, TODO: exception handling... */)
                {
                    currentItem = i; break; // there is a match - store the matching item and break out of the loop
                }
            }

            if (currentItem != null) // if we have found an item
            {
                model.shoppingCart = CheckoutFacilitator.RemoveItemFromCart(currentItem, model);
            }
            Session["model"] = model; // reassign model.
        }
       
        /// <summary>
        /// HttpPost interface for adding an item to the cart
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        [HttpPost]
        public void AddItemToCart(string itemId, string quantity)
        {
            model = (ShopModel)Session["model"]; // get the model object
            Item currentItem = null;
            // For each item in the item catalog, check whether the item's ID matches the
            // input ID.
            foreach (var i in model.itemCatalog)
            {
                if (i.itemID == Int32.Parse(itemId)/*Parse Item ID, TODO: exception handling... */)
                {
                    currentItem = i; break; // there is a match - store the matching item and break out of the loop
                }
            }

            if (currentItem != null) // if we have found an item
            {
                model.shoppingCart = CheckoutFacilitator.AddItemToCart(currentItem, Int32.Parse(quantity), model);
            }
            Session["model"] = model; // reassign model.
        }

        /// <summary>
        /// HttpPost interface for creating an order.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="shippingStreet"></param>
        /// <param name="shippingCity"></param>
        /// <param name="shippingPostcode"></param>
        /// <param name="shippingState"></param>
        /// <param name="billingStreet"></param>
        /// <param name="billingCity"></param>
        /// <param name="billingPostcode"></param>
        /// <param name="billingState"></param>
        /// <param name="billingMethod"></param>
        [HttpPost]
        public void CreateOrder(string firstName, string lastName, string email, string phone, string shippingStreet, string shippingCity,
            string shippingPostcode, string shippingState, string billingStreet, string billingCity, string billingPostcode, string billingState, string billingMethod)
        {
            model = (ShopModel)Session["model"]; // get the model object

            model.currentReceipt = CheckoutFacilitator.CreateOrder(firstName, lastName, email, phone, shippingStreet, shippingCity,
              shippingPostcode, shippingState, billingStreet, billingCity, billingPostcode, billingState, billingMethod, model);

            //clear cart - we don't need it any more!
            model.shoppingCart.Clear();
            Session["model"] = model; // reassign model.
            
        }

        /// <summary>
        /// Loads the minimal order receipt view (displays when order is complete)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadOrderReceiptMinimal()
        {
            model = (ShopModel)Session["model"];
            return PartialView("_OrderReceiptMinimal", model);
        }
        /// <summary>
        /// Loads the minimal shopping cart view (i.e. the cart on top right hand corner)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadShoppingCartMinimalView()
        {
            model = (ShopModel)Session["model"];
            return PartialView("_ShoppingCartMinimalView", model);
        }
        /// <summary>
        /// Loads the Item Catalog partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadItemCatalogView()
        {
            model = (ShopModel)Session["model"];
            return PartialView("_ItemCatalogView", model);
        }

        /// <summary>
        /// Loads the Check out partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadCheckOutView()
        {
            model = (ShopModel)Session["model"];
            return PartialView("_CheckoutView", model);
        }

        /// <summary>
        /// Loads the shopping cart partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadShoppingCartView()
        {
            model = (ShopModel)Session["model"];
            return PartialView("_ShoppingCartView", model);
        }

        /// <summary>
        /// Loads the Item Catalog partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadOrderStatusView()
        {
            model = (ShopModel)Session["model"];
            return PartialView("_OrderStatusView", model);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}