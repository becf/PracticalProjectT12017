using EmbrOnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Controllers.Utilities
{
    public abstract class ItemCatalogFacilitator
    {
        /// <summary>
        /// Gets an item from the database using the item ID.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static Item GetItemByID(int itemID)
        {
            Item item = null;

            // Initialise database connection

            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect

            string query = "SELECT * FROM [DBO].[Item] WHERE [ITEM_ID] = @ITEM_ID"; //sql query
            using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
            {
                cmd.Parameters.Add("@ITEM_ID", SqlDbType.Int, 50).Value = itemID;

                using (SqlDataReader reader = cmd.ExecuteReader()) // execute query
                {
                    while (reader.Read()) // iterate through results
                    {
                        item = new Item();
                        item.itemID = Int32.Parse(reader[0].ToString()); // get item ID
                        item.name = reader[1].ToString(); // get item name
                        item.description = reader[2].ToString(); // get item description
                        item.unitPrice = Double.Parse(reader[3].ToString()); // get item's unit price
                        item.sellingPrice = Double.Parse(reader[4].ToString()); // get item's selling price
                        item.category = reader[5].ToString();  // get item categoy
                        item.imageURL = reader[6].ToString(); // get image URL

                        

                    }
                }
            }
            // Close database connection

            database.CloseDatabaseConnection();
            return item;
        }
        /// <summary>
        /// Retrieves all items from the database and returns them
        /// in a list.
        /// </summary>
        /// <returns></returns>

        public static List<Item> GetAllItems()
        {
            List<Item> items = new List<Item>();

            // Initialise database connection

            DatabaseFacilitator database = new DatabaseFacilitator();

            database.ConnectToDatabase(); // connect

            string query = "SELECT * FROM [DBO].[Item]"; //sql query
            using (SqlCommand cmd = new SqlCommand(query, database.GetDatabaseConnection())) //create sql command
            {

                using (SqlDataReader reader = cmd.ExecuteReader()) // execute query
                {
                    while (reader.Read()) // iterate through results
                    {
                        Item item = new Item();
                        item.itemID = Int32.Parse(reader[0].ToString()); // get item ID
                        item.name = reader[1].ToString(); // get item name
                        item.description = reader[2].ToString(); // get item description
                        item.unitPrice = Double.Parse(reader[3].ToString()); // get item's unit price
                        item.sellingPrice = Double.Parse(reader[4].ToString()); // get item's selling price
                        item.category = reader[5].ToString();  // get item categoy
                        item.imageURL = reader[6].ToString(); // get image URL

                        items.Add(item);

                    }
                }
            }
            // Close database connection

            database.CloseDatabaseConnection();
            return items;


        }
    }
}
