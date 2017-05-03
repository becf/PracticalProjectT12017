using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmbrOnlineStore.Controllers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmbrOnlineStore.Models;

namespace EmbrOnlineStore.Controllers.Utilities.Tests
{
    [TestClass()]
    public class UtilitiesTests
    {
        // ------------- Begin Checkout Facilitator Tests ---------------------
        /// <summary>
        /// CheckOutFacilitator: Unit test for GetCartTotal().
        /// </summary>
        [TestMethod()]
        public void GetCartTotalTest()
        {
            Dictionary<Item, int> cart = new Dictionary<Item, int>();

            Item item1 = new Item
            {
                sellingPrice = 50
            };

            cart.Add(item1, 5); // 5 * 50 = 250

            Item item2 = new Item
            {
                sellingPrice = 20
            };

            cart.Add(item2, 2); // 20 * 2 = 40

            // cart total should be 290.

            var total = CheckoutFacilitator.GetCartTotal(cart);
            if (total != 290)
            {
                Assert.Fail(); // if total not 290, fail test.
            }
        }

        /// <summary>
        /// CheckOutFacilitator: Test adding items to cart.
        /// </summary>
        [TestMethod()]
        public void AddItemToCartTest()
        {
            ShopModel tempModel = new ShopModel();
            tempModel.shoppingCart = new Dictionary<Item, int>();
            Item item1 = new Item
            {
                itemID = 1,
                name = "Item1",
                sellingPrice = 50
            };
            tempModel.shoppingCart.Add(item1, 5); // added 5 of item 1 to cart.
            Item item2 = new Item
            {
                itemID = 2,
                name = "Item2",
                sellingPrice = 50
            };
            CheckoutFacilitator.AddItemToCart(item2, 3, tempModel); // add new item to cart

            // cart should have 8 items total, and 2 entreis in the cart.

            if (tempModel.shoppingCart.Count != 2)
            {
                Assert.Fail();
            }
            if (tempModel.shoppingCart.Sum(x => x.Value) != 8)
            {
                Assert.Fail();

            }
        }
        /// <summary>
        /// CheckOutFacilitator: Test removing items from cart.
        /// </summary>
        [TestMethod()]
        public void RemoveItemFromCartTest()
        {
            ShopModel tempModel = new ShopModel();
            tempModel.shoppingCart = new Dictionary<Item, int>();
            Item item1 = new Item
            {
                itemID = 1,
                name = "Item1",
                sellingPrice = 50
            };
            tempModel.shoppingCart.Add(item1, 5); // added 5 of item 1 to cart.
            Item item2 = new Item
            {
                itemID = 2,
                name = "Item2",
                sellingPrice = 50
            };
            tempModel.shoppingCart.Add(item2, 2); // currently cart should have 8 items total, and 2 entries in the cart.

            // now remove item 2

            CheckoutFacilitator.RemoveItemFromCart(item2, tempModel);

            // now should only have 1 entry, which should be item 1.
            if (tempModel.shoppingCart.Count != 1)
            {
                Assert.Fail();
            }
            if (tempModel.shoppingCart.ContainsKey(item2))
            {
                Assert.Fail();
            }
            if (tempModel.shoppingCart.Sum(x => x.Value) != 5) // should have a qty of 5
            {
                Assert.Fail();

            }
        }
        /// <summary>
        /// CheckOutFacilitator: Test editing qty of an item in the cart.
        /// </summary>
        [TestMethod()]
        public void EditQuantityTest()
        {
            ShopModel tempModel = new ShopModel();
            tempModel.shoppingCart = new Dictionary<Item, int>();
            Item item1 = new Item
            {
                itemID = 1,
                name = "Item1",
                sellingPrice = 50
            };
            tempModel.shoppingCart.Add(item1, 5); // added 5 of item 1 to cart.

            CheckoutFacilitator.EditQuantity(item1, 2, tempModel); // should now have 2 of item1

            if (tempModel.shoppingCart[item1] != 2)
            {
                Assert.Fail();
            }
        }
        // ------------- End Checkout Facilitator Tests ---------------------
        // ------------- Begin Item Catalog Facilitator Tests ---------------
        /// <summary>
        /// ItemCatalogFacilitator: Test filtering items by category.
        /// </summary>
        [TestMethod()]
        public void FilterItemsByCategoryTest()
        {
            List<Item> itemList = new List<Item>();
            Item item1 = new Item
            {
                itemID = 1,
                name = "Item1",
                category = "Category1",
                sellingPrice = 50
            };
            Item item2 = new Item
            {
                itemID = 2,
                name = "Item2",
                category = "Category2",
                sellingPrice = 50
            };
            Item item3 = new Item
            {
                itemID = 3,
                name = "Item3",
                category = "Category1",
                sellingPrice = 50
            };
            itemList.Add(item1);
            itemList.Add(item2);
            itemList.Add(item3);

            List<String> categories = new List<string>();
            categories.Add("Category1");
            itemList = ItemCatalogFacilitator.FilterItemsByCategory(categories, itemList);

            // should have 2 items in the list and they should be item1 and item 3
            if (itemList.Count != 2)
            {
                Assert.Fail();
            }
            if (!(itemList.Contains(item1) && itemList.Contains(item3)))
            {
                Assert.Fail();
            }
        }
        /// <summary>
        /// ItemCatalogFacilitator: Test sorting items, both alphabetically and by price
        /// </summary>
        [TestMethod()]
        public void OrderItemsTest()
        {
            // create dummy items
            List<Item> itemList = new List<Item>();
            Item item1 = new Item
            {
                itemID = 1,
                name = "A",
                sellingPrice = 25
            };
            Item item2 = new Item
            {
                itemID = 2,
                name = "B",
                sellingPrice = 10
            };
            Item item3 = new Item
            {
                itemID = 3,
                name = "C",
                sellingPrice = 100
            };
            itemList.Add(item1);
            itemList.Add(item2);
            itemList.Add(item3);

            itemList = ItemCatalogFacilitator.OrderItems(ItemCatalogFacilitator.ItemSorttype.NAME_AZ, itemList);
            // should be in alphabetical order from A - Z

            if (itemList[0].name != "A")
            {
                Assert.Fail();
            }
            if (itemList[1].name != "B")
            {
                Assert.Fail();
            }
            if (itemList[2].name != "C")
            {
                Assert.Fail();
            }

            // now check alphabetical order Z-A
            itemList = ItemCatalogFacilitator.OrderItems(ItemCatalogFacilitator.ItemSorttype.NAME_ZA, itemList);
            if (itemList[0].name != "C")
            {
                Assert.Fail();
            }
            if (itemList[1].name != "B")
            {
                Assert.Fail();
            }
            if (itemList[2].name != "A")
            {
                Assert.Fail();
            }

            // now check price Low - High
            itemList = ItemCatalogFacilitator.OrderItems(ItemCatalogFacilitator.ItemSorttype.PRICE_LH, itemList);

            // should be in order item2, item1, item3
            if (itemList[0] != item2)
            {
                Assert.Fail();
            }
            if (itemList[1] != item1)
            {
                Assert.Fail();
            }
            if (itemList[2] != item3)
            {
                Assert.Fail();
            }

            // now check price High - Low
            itemList = ItemCatalogFacilitator.OrderItems(ItemCatalogFacilitator.ItemSorttype.PRICE_HL, itemList);

            // should be in order item3, item1, item2
            if (itemList[0] != item3)
            {
                Assert.Fail();
            }
            if (itemList[1] != item1)
            {
                Assert.Fail();
            }
            if (itemList[2] != item2)
            {
                Assert.Fail();
            }
        }
        /// <summary>
        /// ItemCatalogFacilitator: Test searching for items, using a keyword
        /// </summary>
        [TestMethod()]
        public void SearchItemsTest()
        {
            List<Item> itemList = new List<Item>();
            Item item1 = new Item
            {
                itemID = 1,
                name = "A",
                category = "hello",
                description = "bad no",
                sellingPrice = 25
            };
            Item item2 = new Item
            {
                itemID = 2,
                name = "B",
                category = "djdjd",
                description = "bad good fire",
                sellingPrice = 10
            };
            Item item3 = new Item
            {
                itemID = 3,
                name = "C",
                category = "category",
                description = "hello good fire",
                sellingPrice = 100
            };
            itemList.Add(item1);
            itemList.Add(item2);
            itemList.Add(item3);
            // going to search for word "hello"

            itemList = ItemCatalogFacilitator.SearchItems("hello", itemList);
            // item list should only have 2 items now
            if (itemList.Count != 2)
            {
                Assert.Fail();
            }
            // item list shouldn't contain item2
            if (itemList.Contains(item2))
            {
                Assert.Fail();
            }
        }
        // --------------- End Item Catalog Facilitator Tests ---------------

    }
}