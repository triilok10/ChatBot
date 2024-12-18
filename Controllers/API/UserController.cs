﻿using ChatBot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace ChatBot.Controllers.API
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly string _connectionString;
        private readonly string _jwtSecretKey;

        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
            _jwtSecretKey = configuration["Jwt:SecretKey"];
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

                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { res = false, msg = "Token is missing" });
                }


                var principal = ValidateToken(token);
                if (principal == null)
                {
                    return Unauthorized(new { res = false, msg = "Invalid or expired token" });
                }


                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new { res = false, msg = "Invalid UserID in token" });
                }


                if (userId <= 0)
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
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    SqlDataReader rdr = cmd.ExecuteReader();
                    {
                        if (rdr.Read())
                        {
                            obj.UserName = Convert.ToString(rdr["UserName"]);
                            obj.FirstName = Convert.ToString(rdr["FirstName"]);
                            obj.LastName = Convert.ToString(rdr["LastName"]);
                            obj.Message = "Data retrieved successfully";
                            obj.Response = true;
                        }
                        else
                        {
                            obj.Message = "No data found for the user";
                            obj.Response = false;
                        }
                        return Ok(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                res = false;
                return Ok(new { res, msg = ex.Message });
            }
        }
        #endregion

        #region "AskToChatBot"

        [HttpPost]
        public async Task<IActionResult> AskToChatBot([FromBody] AIChat pAIChat)
        {
            AIChat obj = new AIChat();

            obj.Message = string.Empty;
            obj.Response = false;
            try
            {
                string rasaAPIEndpoint = "http://localhost:5005/webhooks/rest/webhook";
                string rasaStatusEndpoint = "http://localhost:5005/status";

                using (var client = new HttpClient())
                {
                    var statusResponse = await client.GetAsync(rasaStatusEndpoint);
                    if (statusResponse.IsSuccessStatusCode)
                    {
                        var requestBody = new
                        {
                            sender = "user",
                            message = pAIChat.userQuestion
                        };

                        var jsonContent = JsonConvert.SerializeObject(requestBody);
                        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        var responseMessage = await client.PostAsync(rasaAPIEndpoint, content);

                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var rasaResponseJson = await responseMessage.Content.ReadAsStringAsync();
                            var rasaResponse = JsonConvert.DeserializeObject<List<RasaResponse>>(rasaResponseJson);

                            if (rasaResponse != null && rasaResponse.Count > 0)
                            {
                                obj.RasaSolution = rasaResponse[0].Text;
                                obj.Response = true;
                            }
                            else
                            {
                                obj.Message = "No response from Rasa.";
                                obj.Response = true;
                            }
                        }
                        else
                        {
                            obj.Message = $"Rasa API error: {responseMessage.StatusCode}";
                            obj.Response = false;
                        }
                    }
                    else
                    {
                        obj.Message = "Rasa server is not running or unreachable.";
                        obj.Response = false;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.Message = $"Error: {ex.Message}";
            }

            return Ok(obj);
        }


        #endregion

        #region "Validate JWT Token"
        private ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "MyAppAPI",
                    ValidAudience = "MyAppClient",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}
