using EmbrOnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Controllers.Utilities
{
    public abstract class CheckoutFacilitator
    {
        public static Dictionary<Item, int> AddItemToCart(Item currentItem, int quantity, ShopModel model)
        {
            // if our shopping cart already contains the item we don't want to
            // duplicate it. so we will add the new quantity to the old quantity.
            if (model.shoppingCart.ContainsKey(currentItem))
            {
                model.shoppingCart[currentItem] += (quantity);
            }
            else
            {
                model.shoppingCart.Add(currentItem, (quantity)); // doesn't exist, add new entry.
            }
            return model.shoppingCart; // return updated shopping cart.
        }

        public static Dictionary<Item, int> RemoveItemFromCart(Item currentItem, ShopModel model)
        {
            // search our shopping cart and remove if present.
            if (model.shoppingCart.ContainsKey(currentItem))
            {
                model.shoppingCart.Remove(currentItem);
            }
            else { /* item doesnt exist, do nothing */}
           
            return model.shoppingCart;// return updated shopping cart.
        }

        public static Dictionary<Item, int> EditQuantity(Item currentItem, int quantity, ShopModel model)
        {
            // Find item in shopping cart and assign it the new quantity.
            if (model.shoppingCart.ContainsKey(currentItem))
            {
                model.shoppingCart[currentItem] = (quantity);
            }
          
            return model.shoppingCart; // return updated shopping cart.
        }
    }
}
