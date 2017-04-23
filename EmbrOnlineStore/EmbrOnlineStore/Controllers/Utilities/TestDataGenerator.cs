using EmbrOnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Controllers.Utilities
{
   public  abstract class TestDataGenerator
    {
        public static ShopModel PopulateDummyModel()
        {
            ShopModel model = new ShopModel();

            model.customer = PopulateDummyCustomer();
            model.itemCatalog = PopulateDummyItemCatalog();
            model.shoppingCart = new Dictionary<Item, int>();
            model.shoppingCart.Add(model.itemCatalog[1], 2);
          
            return model;
        }

        public static Customer PopulateDummyCustomer()
        {
            Customer testCustomer = new Customer();
            testCustomer.name = "Joe Bloggs";
            testCustomer.paymentMethod = PayentMethodEnum.PayPal;
            testCustomer.phone = "1234567890";
            testCustomer.address = "1 Somewhere St";
            testCustomer.customerID = 1;
            return testCustomer;
        }

        public static List<Item> PopulateDummyItemCatalog()
        {
            List<Item> itemCatalog = new List<Item>();
            
            for(int i = 1; i < 5; i++)
            {
                Item item = new Item();
                item.itemID = i;
                item.name = "item" + i;
                item.description = "description of item " + i;
                item.category = "Category" + (i % 2);
                item.imageURL = @"\Content\Images\Dummy\"+i+".png";
                item.unitPrice = i * 20;
                item.sellingPrice = item.unitPrice * 1.5;
                itemCatalog.Add(item);
            }

            return itemCatalog;
        }
    }
}
