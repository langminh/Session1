using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace Sesssion1.Code
{
    public class DTA
    {
        public string connectionString
        {
            get
            {
                return @"Data Source=DESKTOP-OPR90UQ;Initial Catalog=session1;Integrated Security=True";
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(this.connectionString);
        }
    }
}
