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
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Loads the Item Catalog partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadItemCatalogView()
        {
            return PartialView("_ItemCatalogView");
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