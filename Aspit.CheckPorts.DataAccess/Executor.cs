using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Aspit.CheckPorts.Entities;

namespace Aspit.CheckPorts.DataAccess
{
    public class Executor
    {
        #region Fields
        /// <summary>The connection string.</summary>
        private static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Aspit.Port;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //private static string connectionString = @"Data Source=10.143.79.127\MSSQLLocalDB;Initial Catalog=Aspit.Port;Integrated Security=True;Connect Timeout=30;Encrypt=False";
        #endregion


        #region Constructor
        /// <summary>Creates a new <see cref="Executor"/> object. Tries to connecet. If fail throw Exception</summary>
        public Executor()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region Methods
        /// <summary>
        /// Executes a sqlquery string, changes database
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet Execute(string sqlQuery)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            DataSet set = new DataSet(" ");
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(set);
            }
            return set;
        }
        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="procedureName">The stored procedure that you want to run</param>
        /// <param name="sqlQuery">SP Parameter</param>
        public void Execute(string procedureName, string sqlQuery)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("SqlQuery", sqlQuery);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        #endregion       
    }
}
