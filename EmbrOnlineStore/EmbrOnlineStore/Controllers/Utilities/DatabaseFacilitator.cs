/*******************************************************************************************************
* SIT782 - PRACTICAL PROJECT T1 2017
*
* GROUP 13:
*           1. REBECCA FRITH (ID: 213582268)
*           2. ERIC GRIGSON (ID: 212415996)
*           3. BENJAMIN FRIEBE (ID: 217109315)    
*
* ------------------------------------------------------------------------------------------------------
* FILE NAME:        DATABASEFACILITATOR.CS
* FILE DESCRIPTION: Connects to database using existing ConnectionString (in Web.config). Also 
*                   safely facilitates connecting to database and closing the database connection.
********************************************************************************************************/
using System;
using System.Data.SqlClient;

namespace EmbrOnlineStore.Controllers.Utilities
{
    /// <summary>
    /// Generic exception wrapper for database connectivity
    /// </summary>
    public class DatabaseException : Exception
    {
        public DatabaseException() { }
    }
    public class DatabaseFacilitator 
    {
        // connection string is taken from web.config - should point to user's relative App_Data folder.
        protected string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EMBRDB"].ConnectionString; 

        // connection object
        private SqlConnection connection;
       
        /// <summary>
        /// Retrieves an existing database connection
        /// </summary>
        /// <returns></returns>
       public SqlConnection GetDatabaseConnection()
        {
            return connection;
        }
        /// <summary>
        /// Creates a new database connection, using a connection string as input
        /// </summary>
        public DatabaseFacilitator()
        {
            connection = new SqlConnection(connectionString);
        }
        /// <summary>
        /// Connects to existing database, throws a DatabaseException if not open
        /// </summary>
        public void ConnectToDatabase()
        {
            if (connection == null || connection.State == System.Data.ConnectionState.Open)
            {
                throw new DatabaseException();
            }
            else
                connection.Open();
        }
        /// <summary>
        /// CLose database connection
        /// </summary>
        public void CloseDatabaseConnection()
        {
            if (connection == null || connection.State != System.Data.ConnectionState.Open)
            {
                throw new DatabaseException();
            }   
            else
            {
                connection.Close();
            }
        }
    }
}
