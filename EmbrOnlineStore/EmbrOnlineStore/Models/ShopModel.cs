/*******************************************************************************************************
* SIT782 - PRACTICAL PROJECT T1 2017
*
* GROUP 13:
*           1. REBECCA FRITH (ID: 213582268)
*           2. ERIC GRIGSON (ID: 212415996)
*           3. BENJAMIN FRIEBE (ID: 217109315)    
*
* ------------------------------------------------------------------------------------------------------
* FILE NAME:        SHOPMODEL.CS
* FILE DESCRIPTION: Model component of the MVC paradigm. Contains all information required to 
*                   maintain the state of the online store.
********************************************************************************************************/
using System.Collections.Generic;

namespace EmbrOnlineStore.Models
{
    public class ShopModel
    {

        /// <summary>
        /// Default constructors for ShopModel
        /// </summary>
        public ShopModel()
        {
            itemCatalog = new List<Item>();
            shoppingCart = new Dictionary<Item, int>();
        }

        /// <summary>
        /// Constructor which takes an existing ShopModel and creates a new one
        /// based on it.
        /// </summary>
        /// <param name="shopModel"></param>
        public ShopModel(ShopModel shopModel) { }

        public List<Item> itemCatalog { get; set; } // list of items
        public Item selectedItem { get; set; } // selected item 
        public Dictionary<Item, int> shoppingCart { get; set; } // A dictionary of Items and their quantities
        public Customer customer { get; set; } //current customer
        public Order currentOrder { get; set; } //retrieved order
        public Receipt currentReceipt { get; set; } // current order receipt


    }
}
