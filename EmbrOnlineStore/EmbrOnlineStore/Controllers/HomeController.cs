/*******************************************************************************************************
* SIT782 - PRACTICAL PROJECT T1 2017
*
* GROUP 13:
*           1. REBECCA FRITH (ID: 213582268)
*           2. ERIC GRIGSON (ID: 212415996)
*           3. BENJAMIN FRIEBE (ID: 217109315)    
*
* ------------------------------------------------------------------------------------------------------
* FILE NAME:        HomeController.cs
* FILE DESCRIPTION: This is the Controller aspect of the MVC pattern. It contains all functionality
*                   to render each of the Views and partial views required for the online shop.
*                   Detailed functionality (such as database connectivity) is farmed out to the 
*                   classes in the Utilities folder.
*
********************************************************************************************************/

using EmbrOnlineStore.Controllers.Utilities;
using EmbrOnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmbrOnlineStore.Controllers
{
    public class HomeController : Controller
    {
        public ShopModel model;
        /// <summary>
        /// Loads the main Index view. 
        /// 
        /// Creates a new model and loads the item catalog from the database.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            
             Session["fromSearch"] = false;
            //model= TestDataGenerator.PopulateDummyModel();
            model = new ShopModel();
            model.itemCatalog = ItemCatalogFacilitator.GetAllItems();
            Session["model"] = model;
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

        [HttpPost]
        public void RetrieveOrder(string orderID)
        {
            ShopModel model = (ShopModel)Session["model"];
            model.currentOrder =  OrderRetrievalFacilicator.GetOrderByID(Int32.Parse(orderID));
            Session["model"] = model;
        }
        /// <summary>
        /// Orders the item catalog and saves to model.
        /// </summary>
        /// <param name="orderBy"></param>
        [HttpPost]
        public void OrderItemsCatalog(string orderBy)
        {
            ShopModel model = (ShopModel)Session["model"];

            ItemCatalogFacilitator.ItemSorttype type = ItemCatalogFacilitator.ItemSorttype.NAME_AZ;
            switch (orderBy)
            {
                case "PRICE_HL":
                    type = ItemCatalogFacilitator.ItemSorttype.PRICE_HL;
                    break;
                case "PRICE_LH":
                    type = ItemCatalogFacilitator.ItemSorttype.PRICE_LH;
                    break;
                case "NAME_AZ":
                    type = ItemCatalogFacilitator.ItemSorttype.NAME_AZ;
                    break;
                case "NAME_ZA":
                    type = ItemCatalogFacilitator.ItemSorttype.NAME_ZA;
                    break;
            }
        model.itemCatalog =    ItemCatalogFacilitator.OrderItems(type, model.itemCatalog);
            Session["model"] = model;
        }
        /// <summary>
        /// Filters (by category) the item catalog and saves to model.
        /// </summary>
        /// <param name="categories"></param>
        [HttpPost]
        public void FilterItemsByCategory(string categories)
        {
            ShopModel model = (ShopModel)Session["model"];


            model.itemCatalog = ItemCatalogFacilitator.GetAllItems();
            List<string> catList = new List<string>();
            string[] cat = categories.Split(',');
            for (int i = 0; i < cat.Length; i++)
                catList.Add(cat[i]);


            model.itemCatalog = ItemCatalogFacilitator.FilterItemsByCategory(catList, model.itemCatalog); 
            Session["model"] = model;
        }
        /// <summary>
        /// Searches for items that contain a search stringand saves them to model.
        /// </summary>
        /// <param name="searchTerm"></param>
        [HttpPost]
        public void SearchItems(string searchTerm)
        {
            ShopModel model = (ShopModel)Session["model"];
            Session["searchTerm"] = searchTerm;

            model.itemCatalog = ItemCatalogFacilitator.GetAllItems(); // reset
            model.itemCatalog = ItemCatalogFacilitator.SearchItems(searchTerm, model.itemCatalog);
            Session["fromSearch"] = true;
            Session["model"] = model;
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
            // test to finish shopping cart -- remove

            model.shoppingCart = new Dictionary<Item, int>();
            model.shoppingCart.Add(ItemCatalogFacilitator.GetItemByID(1), 5);
            model.shoppingCart.Add(ItemCatalogFacilitator.GetItemByID(4), 2);
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
        /// <summary>
        /// Loads the Order seardch results partial view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadOrderSearchResultsView()
        {
            model = (ShopModel)Session["model"];
            return PartialView("_OrderSearchResults", model);
        }

        /// <summary>
        /// Loads just the item catalog table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LoadCatalogItems()
        {
            model = (ShopModel)Session["model"];
            return PartialView("_CatalogItemsView", model);
        }
       
    }
}