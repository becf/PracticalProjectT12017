using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Models
{
    public class Item
    {
        public int itemID { get; set; } // item identifier
        public string name { get; set; } // name of item
        public string description { get; set; } // description of item
        public double unitPrice { get; set; } // unit price of item
        public double sellingPrice {get;set;} // selling price of item
        public string category { get; set; }
        public string imageURL { get; set;  } // URL of catalog image
        /// <summary>
        /// Default constructor for Item object.
        /// </summary>
        Item()
        {
            // intentionally empty
        }

        /// <summary>
        /// Item constructor containing required properties. Sets object properties based on inputs.
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="unitPrice"></param>
        /// <param name="sellingPrice"></param>
        /// <param name="category"></param>
        /// <param name="itemURL"></param>
        /// 
        Item(string name, string description, string category, double unitPrice, double sellingPrice, string imageURL)
        {
            this.name = name;
            this.description = description;
            this.unitPrice = unitPrice;
            this.category = category;
            this.sellingPrice = sellingPrice;
            this.imageURL = imageURL;
        }

        public Item GetItem(int itemID)
        {
            // Connect to database
            // SqlConnection sql = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\pp2017tri1\Source\Repos\PracticalProjectT12017\EmbrOnlineStore\EmbrOnlineStore\App_Data\EmbrDB.mdf;Integrated Security=True");


            // Get Item from DB by ID

            // Return Item

            return new Item("test", "test", "Hoses", 50.0, 70.0, "null");
        }
        public void SetItem(Item item)
        {
            // implement if needed
        }
    }
}
