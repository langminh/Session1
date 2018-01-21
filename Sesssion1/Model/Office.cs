using Sesssion1.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesssion1.Model
{
    public class Office
    {
        public int ID { get; set; }
        public int CountryID { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Contact { get; set; }

        public Office()
        {
            this.ID = -1;
            this.Title = string.Empty;
            this.Phone = string.Empty;
            this.Contact = string.Empty;
        }
    }

    public class OfficeModel
    {
        public string query
        {
            get
            {
                return @"select [ID]
      ,[CountryID]
      ,[Title]
      ,[Phone]
      ,[Contact] from Offices";
            }
        }

        public static string queryi
        {
            get
            {
                return @"select [ID]
      ,[CountryID]
      ,[Title]
      ,[Phone]
      ,[Contact] from Offices";
            }
        }

        public Office GetOffice(DataRow dr)
        {
            Office result = new Office();
            result.ID = int.Parse(dr["ID"].ToString());
            result.CountryID = int.Parse(dr["CountryID"].ToString());
            result.Title = dr["Title"].ToString();
            result.Phone = dr["Phone"].ToString();
            result.Contact = dr["Contact"].ToString();
            return result;
        }

        public static Office GetOfficeI(DataRow dr)
        {
            Office result = new Office();
            result.ID = int.Parse(dr["ID"].ToString());
            result.CountryID = int.Parse(dr["CountryID"].ToString());
            result.Title = dr["Title"].ToString();
            result.Phone = dr["Phone"].ToString();
            result.Contact = dr["Contact"].ToString();
            return result;
        }

        public static Office GetOfficeByID(int ID)
        {
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlDataAdapter adapter = null;
            string sqlQuery = queryi + " where ID = @id;";
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            cmd.Parameters.Add(new SqlParameter("id", ID));
            try
            {
                conn.Open();
                adapter = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                adapter.Fill(t);
                if (t.Rows.Count > 0)
                {
                    return GetOfficeI(t.Rows[0]);
                }
            }
            catch { }
            finally
            {
                adapter.Dispose();
                conn.Close();
            }
            return null;
        }

        public List<Office> GetOffices()
        {
            List<Office> list = new List<Office>();
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlDataAdapter adapter = null;
            try
            {
                conn.Open();
                adapter = new SqlDataAdapter(query, conn);
                DataTable t = new DataTable();
                adapter.Fill(t);
                if (t.Rows.Count > 0)
                {
                    foreach (DataRow dr in t.Rows)
                    {
                        list.Add(GetOffice(dr));
                    }
                }
            }
            catch { }
            finally
            {
                adapter.Dispose();
                conn.Close();
            }
            return list;
        }
    }
}
