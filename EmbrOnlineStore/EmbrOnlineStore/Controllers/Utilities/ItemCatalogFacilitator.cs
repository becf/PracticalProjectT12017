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
        public enum ItemSorttype
        {
            PRICE_HL,
            PRICE_LH,
            NAME_AZ,
            NAME_ZA
        }
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

        /// <summary>
        /// Filteres items by their category label. Takes a list of categories to include and
        /// the original item list and returns a filtered list.
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="originalList"></param>
        /// <returns></returns>
        public static List<Item> FilterItemsByCategory(List<string> categories, List<Item> originalList)
        {
            List<Item> filtered = new List<Item>();

            foreach (var i in originalList)
            {
                if (categories.Contains(i.category)) //if item category is in our list of included categories, add to filtered list.
                {
                    filtered.Add(i);
                }
            }
            return filtered;
        }
        /// <summary>
        /// Uses LINQ queries to sort a list based on a sort type and the original list.
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="originalList"></param>
        /// <returns></returns>
        public static List<Item> OrderItems(ItemSorttype orderBy, List<Item> originalList  )
        {
            List<Item> sortedList = new List<Item>();
            switch (orderBy)
            {
                case ItemSorttype.NAME_AZ:
                    sortedList = originalList.OrderBy(o => o.name).ToList();

                    break;
                case ItemSorttype.NAME_ZA:
                    sortedList = originalList.OrderByDescending(o => o.name).ToList();
                    break;
                case ItemSorttype.PRICE_LH:
                    sortedList = originalList.OrderBy(o => o.sellingPrice).ToList();
                    break;
                case ItemSorttype.PRICE_HL:
                    sortedList = originalList.OrderByDescending(o => o.sellingPrice).ToList();
                    break;

            }
            return sortedList;
        }

        /// <summary>
        /// Takes a search term and searches the entire item catalog to find any 
        /// matches. Returns the filtered list.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="originalList"></param>
        /// <returns></returns>
        public static List<Item> SearchItems(string searchTerm, List<Item> originalList)
        {
            List<Item> filteredList = new List<Item>();
            foreach(Item i in originalList)
            {
                // not doing any fancy fuzzy matching, just checking if search term exists
                // anywhere in item object.
                if (i.description.Contains(searchTerm) || i.category.Contains(searchTerm)|| i.name.Contains(searchTerm) ||i.itemID.ToString().Contains(searchTerm))
                {
                    filteredList.Add(i);
                }
            }
            return filteredList;
        }
    }
}
