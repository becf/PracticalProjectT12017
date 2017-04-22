using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Models
{
    public class ShopModel
    {
        
        /// <summary>
        /// Default constructors for ShopModel
        /// </summary>
        ShopModel() { }

        ShopModel(ShopModel shopModel) { }

        public List<Item> itemCatalog { get; set; } // list of items
        public Item selectedItem { get; set; } // selected item 
        public Dictionary<Item, int> shoppingCart { get;set;} // A dictionary of Items and their quantities
        public Customer customer { get; set; } //current customer
        public Order currentOrder { get; set; } //retrieved order
        public Receipt currentReceipt { get; set; } // current order receipt


    }
}
