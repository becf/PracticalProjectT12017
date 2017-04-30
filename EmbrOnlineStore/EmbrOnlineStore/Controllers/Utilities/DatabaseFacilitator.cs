using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbrOnlineStore.Controllers.Utilities
{
    public class DatabaseFacilitator 
    {
        protected string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EMBRDB"].ConnectionString; //@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\pp2017tri1\Source\Repos\PracticalProjectT12017\EmbrOnlineStore\EmbrOnlineStore\App_Data\EmbrDB.mdf;Integrated Security=True";


        private SqlConnection connection;
       
       public SqlConnection GetDatabaseConnection()
        {
            return connection;
        }
        public DatabaseFacilitator()
        {
            connection = new SqlConnection(connectionString);
        }
        public void ConnectToDatabase()
        {
            connection.Open();
        }
        public void CloseDatabaseConnection()
        {
            connection.Close();
        }
    }
}
