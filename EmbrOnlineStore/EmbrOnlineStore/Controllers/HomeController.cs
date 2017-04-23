using EmbrOnlineStore.Controllers.Utilities;
using EmbrOnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

        [HttpPost]
        public void AddItemToCart(string itemId, string quantity)
        {
            model = (ShopModel) Session["model"]; // get the model object
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
        /// Loads the Item Catalog partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadItemCatalogView()
        {
            model = TestDataGenerator.PopulateDummyModel();
            return PartialView("_ItemCatalogView", model);
        }

        /// <summary>
        /// Loads the Check out partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadCheckOutView()
        {
            return PartialView("_CheckoutView");
        }

        /// <summary>
        /// Loads the shopping cart partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadShoppingCartView()
        {
            return PartialView("_ShoppingCartView");
        }

        /// <summary>
        /// Loads the Item Catalog partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadOrderStatusView()
        {
            return PartialView("_OrderStatusView");
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