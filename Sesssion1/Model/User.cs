using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Sesssion1.Code;

namespace Sesssion1.Model
{
    public class User
    {
        public int ID { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int roleID { get; set; }
        public int officeID { get; set; }
        public string countries { get; set; }
        public bool isActive { get; set; }
        public DateTime birthDate { get; set; }

        public User()
        {
            this.ID = -1;
            this.firstName = string.Empty;
            this.lastName = string.Empty;
            this.roleID = 0;
        }
    }

    public class UserModel
    {
        public string query
        {
            get
            {
                return @"select [ID]
      ,[RoleID]
      ,[Email]
      ,[Password]
      ,[FirstName]
      ,[LastName]
      ,[OfficeID]
      ,[Birthdate]
      ,[Active], (select [Name] from Countries,
        Offices where OD.OfficeID = Offices.ID and 
        Countries.ID = Offices.CountryID) country";
            }
        }

        public string GetName(int id)
        {
            string sqlQuery = "select LastName from Users where ID = @id";
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                cmd.Parameters.Add(new SqlParameter("id", id));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                adapter.Fill(t);
                if (t.Rows.Count > 0)
                {
                    return t.Rows[0]["LastName"].ToString();
                }
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return string.Empty;
        }

        public int CheckUser(string username, string password)
        {
            string sqlQuery = "select count(*) from [dbo].[Users] where [Email] = @email";
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                cmd.Parameters.Add(new SqlParameter("email", username));
                int kq = (int)cmd.ExecuteScalar();
                if (kq <= 0)
                {
                    return -2;
                }
                else
                {
                    sqlQuery += " and [Password] = @password";
                    cmd.Dispose();
                    conn.Close();
                    cmd = new SqlCommand(sqlQuery, conn);
                    conn.Open();
                    kq = 0;
                    cmd.Parameters.AddRange(new object[] { new SqlParameter("email", username),
                    new SqlParameter("password", password)});
                    kq = (int)cmd.ExecuteScalar();
                    if (kq <= 0)
                    {
                        return 0;
                    }
                    else
                    {
                        sqlQuery += " and Active = 1";
                        cmd.Dispose();
                        conn.Close();
                        cmd = new SqlCommand(sqlQuery, conn);
                        conn.Open();
                        kq = 0;
                        cmd.Parameters.AddRange(new object[] { new SqlParameter("email", username),
                    new SqlParameter("password", password)});
                        kq = (int)cmd.ExecuteScalar();
                        if (kq <= 0)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return -3;
        }

        public Log GetLog(DataRow dr)
        {
            Log result = new Log();
            result.logID = int.Parse(dr["ID"].ToString());
            result.userID = int.Parse(dr["UserID"].ToString());
            result.timeLogin = (DateTime)dr["TimeLogin"];
            try
            {
                if (dr["CrashID"] != null)
                {
                    int t = 0;
                    if (int.TryParse(dr["CrashID"].ToString(), out t))
                    {
                        result.crashID = t;
                    }
                }
                if (dr["TimeLogout"] != null)
                {
                    DateTime t;
                    if (DateTime.TryParse(dr["TimeLogout"].ToString(), out t))
                    {
                        result.timeLogout = t;
                    }
                }
                if (dr["Description"] != null)
                {
                    result.description = dr["Description"].ToString();
                }
            }
            catch { }
            return result;
        }

        public List<Log> getAllLogByID(int id)
        {
            List<Log> list = new List<Log>();
            string sqlQuery = @"select [ID]
      ,[UserID]
      ,[TimeLogin]
      ,[TimeLogout]
      ,[CrashID]
      ,[Description] from [Log] OD where UserID =  @id Order by TimeLogin DESC";
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                cmd.Parameters.Add(new SqlParameter("id", id));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                adapter.Fill(t);
                int J = 0;
                
                foreach(DataRow i in t.Rows)
                {
                    list.Add(GetLog(i));
                   
                }
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return list;
        }

        public Log getLog(User user)
        {
            Log result = new Log();
            string sqlQuery = @"select [ID]
      ,[UserID]
      ,[TimeLogin]
      ,[TimeLogout]
      ,[CrashID]
      ,[Description] from [Log] OD where UserID =  @id Order by TimeLogin DESC";

            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                cmd.Parameters.Add(new SqlParameter("id", user.ID));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                adapter.Fill(t);
                result = GetLog(t.Rows[0]);
                adapter.Dispose();
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return result;
        }

        public bool UpdateLog(User user, Log log)
        {
            string sqlQuery = @"UPDATE [dbo].[Log]
   SET [UserID] = @userID
      ,[TimeLogout] = Getdate()
 WHERE ID = @id";
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(new object[] { new SqlParameter("userID", user.ID),
                new SqlParameter("id", log.logID)});
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return false;
        }

        public Log getTheFirstRowBefor(User user)
        {
            Log result = new Log();
            string sqlQuery = @"SELECT * FROM 
(
     SELECT [Log].*, 
     ROW_NUMBER() OVER (ORDER BY TimeLogin DESC) AS RANK
     FROM [Log] where UserID = @id
) T 
WHERE T.RANK=2";
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                cmd.Parameters.Add(new SqlParameter("id", user.ID));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                adapter.Fill(t);
                if(t.Rows.Count > 0)
                {
                    return GetLog(t.Rows[0]);
                }
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return null;
        }

        public bool UpdateLogDetail(Log log)
        {


            string sqlQuery = @"UPDATE [dbo].[Log]
   SET [CrashID] = @crash
      ,[Description] = @detail
 WHERE ID = @id";

            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(new object[] { new SqlParameter("crash", log.crashID),
                new SqlParameter("id", log.logID),
                new SqlParameter("detail", log.description)});
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return false;
        }

        public void WriteLog(User user)
        {
            string sqlQuery = @"insert into [Log](UserID, TimeLogin) values(@username, @time)";
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(new object[] { new SqlParameter("username", user.ID),
                    new SqlParameter("time", DateTime.Now)});
                cmd.ExecuteNonQuery();
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        public User GetUser(string username, string password)
        {
            string sqlQuery = query + " from [Users] OD where [Email] = @email and [Password] = @pass and Active = 1";
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();

                cmd.Parameters.AddRange(new object[] { new SqlParameter("email", username),
                    new SqlParameter("pass", password)});
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                adapter.Fill(t);
                foreach (DataRow dr in t.Rows)
                {
                    return this.GetUser(dr);
                }
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return null;
        }

        public User GetUser(string email, string first, string last)
        {
            string sqlQuery = query + " FROM [dbo].[Users] OD where Email = @email and FirstName = @first and LastName = @last";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = new SqlConnection(new DTA().connectionString);
                cmd = new SqlCommand(sqlQuery, conn);

                cmd.Parameters.Add(new SqlParameter("email", email));
                cmd.Parameters.Add(new SqlParameter("first", first));
                cmd.Parameters.Add(new SqlParameter("last", last));
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                adapter.Fill(t);
                if (t.Rows.Count > 0)
                {
                    return GetUser(t.Rows[0]);
                }
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return null;
        }

        public User GetUser(DataRow dr)
        {
            User result = new User();
            result.ID = int.Parse(dr["ID"].ToString());
            result.email = dr["Email"].ToString();
            result.password = dr["Password"].ToString();
            result.firstName = dr["FirstName"].ToString();
            result.lastName = dr["LastName"].ToString();
            result.roleID = int.Parse(dr["RoleID"].ToString());
            result.officeID = int.Parse(dr["OfficeID"].ToString());
            result.countries = dr["country"].ToString();
            result.isActive = bool.Parse(dr["Active"].ToString());
            result.birthDate = (DateTime)dr["Birthdate"];
            return result;
        }

        public bool CheckUser(string email)
        {
            string sqlQuery = @"SELECT COUNT(*) FROM [dbo].[Users] WHERE Email = @email";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = new SqlConnection(new DTA().connectionString);
                cmd = new SqlCommand(sqlQuery, conn);

                cmd.Parameters.Add(new SqlParameter("email", email));
                conn.Open();
                int result = (int)cmd.ExecuteScalar();
                if (result > 1)
                {
                    return true;
                }
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return false;
        }


        public List<User> GetAllUsers(int OfficeID)
        {
            List<User> list = new List<User>();
            SqlConnection conn = new SqlConnection(new DTA().connectionString);
            string sqlQuery = query + " from [dbo].[Users] OD";

            if (OfficeID > 0)
            {
                sqlQuery += " where OfficeID = @officeID";
            }
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            if (OfficeID > 0)
            {
                cmd.Parameters.Add(new SqlParameter("officeID", OfficeID));
            }
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                adapter.Fill(t);
                foreach (DataRow dr in t.Rows)
                {
                    list.Add(GetUser(dr));
                }
                adapter.Dispose();
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return list;
        }

        public bool InsertUser(User user)
        {
            bool result = false;
            string sqlQuery = @"INSERT INTO [dbo].[Users]
           ([RoleID] ,[Email] , [Password], [FirstName] ,[LastName], [OfficeID], [Birthdate], [Active])
     VALUES ( 2, @email, @pass , @first, @last, @officeID, @birth,1)";
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(new DTA().connectionString);
                cmd = new SqlCommand(sqlQuery, conn);

                cmd.Parameters.AddRange(new object[] { new SqlParameter("email", user.email),
                new SqlParameter("pass", encryptionMD5.md5(user.password)),
                new SqlParameter("first", user.firstName),
                new SqlParameter("last", user.lastName),
                new SqlParameter("officeID", user.officeID),
                new SqlParameter("birth", user.birthDate)});
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return result;
        }

        public bool UpdateUser(User user)
        {
            bool result = false;
            string sqlQuery = @"UPDATE [dbo].[Users]
   SET [RoleID] = @role,[Email] = @email,[Password] = @pass,[FirstName] = @first,
        [LastName] = @last,[OfficeID] = @officeID,[Birthdate] = @birth,[Active] = @active
    WHERE ID = @id";

            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = new SqlConnection(new DTA().connectionString);
                cmd = new SqlCommand(sqlQuery, conn);

                cmd.Parameters.AddRange(new object[] { new SqlParameter("email", user.email),
                new SqlParameter("pass", encryptionMD5.md5(user.password)),
                new SqlParameter("first", user.firstName),
                new SqlParameter("last", user.lastName),
                new SqlParameter("officeID", user.officeID),
                new SqlParameter("birth", user.birthDate),
                new SqlParameter("active", user.isActive),
                new SqlParameter("role", user.roleID),
                new SqlParameter("id", user.ID)});
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return result;
        }

        public bool DeleteUser(int ID)
        {
            string sqlQuery = @"DELETE FROM [dbo].[Users] WHERE ID = @id";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = new SqlConnection(new DTA().connectionString);
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.Add(new SqlParameter("id", ID));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch { }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return false;
        }
    }
}
