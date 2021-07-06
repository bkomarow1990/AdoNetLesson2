using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AdoNetLesson2
{
    class SalesDBConnect
    {
        static public SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SalesDB"].ConnectionString);
        public SqlCommand command = new SqlCommand("Select * from Sales",connection);
        public SalesDBConnect()
        {
            connection.Open();
        }
    }
}
