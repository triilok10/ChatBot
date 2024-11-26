﻿using ChatBot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace ChatBot.Controllers.API
{
    //Route for API Endpoint
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginAPIController : ControllerBase
    {
        #region "Connection String"
        private readonly string _connectionString;

        public LoginAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
        }
        #endregion


        #region "Register User"
        [HttpPost]
        public IActionResult RegisterUser(AIUser pAIuser)
        {
            bool res = false;
            string msg = "";
            try
            {
                AIUser Obj = new AIUser();

                using (SqlConnection con = new SqlConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_AIUser", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", 1);
                    cmd.Parameters.AddWithValue("@FirstName", pAIuser.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", pAIuser.LastName);
                    cmd.Parameters.AddWithValue("@EmailID", pAIuser.EmailID);
                    cmd.Parameters.AddWithValue("@PhoneNo", pAIuser.PhoneNo);
                    cmd.Parameters.AddWithValue("@Password", pAIuser.Password);
                    cmd.Parameters.AddWithValue("@Gender", pAIuser.Gender);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Obj.AIUserID = Convert.ToInt32(rdr["AIUserID"]);
                            Obj.UserName = Convert.ToString(rdr["UserName"]);
                        }
                    }
                    return Ok(Obj);

                }


            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;

                return Ok(new { msg, res });
            }
            #endregion

        }
    }
}