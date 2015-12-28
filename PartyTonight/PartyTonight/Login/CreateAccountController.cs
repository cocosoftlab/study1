using System.Web.Http;
using System.Data;
using System.Data.SqlClient;

namespace PartyTonight.Login
{
    public class CreateAccountController : ApiController
    {
        private int IdLengthMax = 128;
        private int PasswordLengthMax = 32;

        private bool CheckValidId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            return id.Length <= IdLengthMax;
        }

        private bool CheckValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            return password.Length <= PasswordLengthMax;
        }


        // POST api/values 
        public string Post(CreateAccountRequestData requestData)
        {
            string errorString = null;

            if (CheckValidId(requestData.id) == false)
            {
                return "Invalid ID";
            }

            if (CheckValidPassword(requestData.password) == false)
            {
                return "Invalid Password";
            }

            var connectionString = "Data Source=220.76.123.23,41433;Initial Catalog=peter;user id=sa;Password=qltmzlt##00";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    /// 이미 가입된 아이디 인지 확인
                    command.CommandText = string.Format("SELECT [uid] FROM [dbo].[Account] WHERE [id] = '{0}'", requestData.id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int uid = reader.GetInt32(0);
                            if (uid > 0)
                            {
                                errorString = "이미 존재하는 아이디입니다.";
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(errorString))
                    {
                        command.CommandText = "INSERT INTO [dbo].[Account] (id, password, sign_in_date) VALUES (@id, @password, @datetime)";
                        command.Parameters.AddWithValue("@id", requestData.id);
                        command.Parameters.AddWithValue("@password", requestData.password);
                        command.Parameters.AddWithValue("@datetime", System.DateTime.UtcNow);
                        command.ExecuteNonQuery();
                    }
                }
            }

            if (!string.IsNullOrEmpty(errorString))
            {
                return errorString;
            }

            return "success";
        }
    }
}
