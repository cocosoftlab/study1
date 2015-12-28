using System.Collections.Generic;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;

namespace PartyTonight.Login
{
    public class LoginController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public string Post(LoginRequestData requestData)
        {
            if (string.IsNullOrEmpty(requestData.id))
            {
                return "Invalid ID";
            }

            int? uid = null;
            string password = null;

            var connectionString = "Data Source=220.76.123.23,41433;Initial Catalog=peter;user id=sa;Password=qltmzlt##00";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "uspFindAccountWithId";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", requestData.id);
                    
                    //command.CommandText = string.Format("SELECT [uid], [password] FROM [dbo].[Account] WHERE [id] = '{0}';or 1=1", requestData.id);
                    //command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            uid = reader.GetInt32(0);
                            password = reader.GetString(1);
                        }
                    }
                }
            }

            if (!uid.HasValue)
            {
                return "No Exist";
            }

            if (password.Equals(requestData.password) == false)
            {
                return "Wrong Password";
            }

            return "로그인 성공";
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
