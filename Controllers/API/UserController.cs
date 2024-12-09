using ChatBot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ChatBot.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly string _connectionString;
        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
        }


        #region "User Record Get"
        [HttpPost]
        public IActionResult UserRecordGet([FromBody] AIChat pAIChat)
        {
            bool res = false;
            string msg = string.Empty;
            AIChat obj = new AIChat();
            try
            {
                if (pAIChat.UserID <= 0)
                {
                    res = false;
                    msg = "Please login";
                    return Ok(new { res, msg });
                }

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ChatUser", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", 1);
                    cmd.Parameters.AddWithValue("@UserID", pAIChat.UserID);

                    SqlDataReader rdr = cmd.ExecuteReader();
                    {
                        if (!rdr.Read())
                        {
                            obj.UserName = Convert.ToString(rdr["UserName"]);
                            obj.FirstName = Convert.ToString(rdr["FirstName"]);
                            obj.LastName = Convert.ToString(rdr["LastName"]);
                            obj.Message = "Data retrived successfully";
                            obj.Response = true;
                        }
                        return Ok(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                res = false;
                return Ok(new { res, msg });
            }
        }

        #endregion
    }
}
