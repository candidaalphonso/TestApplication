using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TestApplication.Models;

namespace TestApplication.Controllers
{
    public class AuthenticationController : Controller
    {
      
        public IActionResult Index()
        {

            return View();
        }

        //view register page
        public IActionResult Register()
        {
          
            return View();
        }
        //view login page
        public IActionResult Login()
        {

            return View();
        }

        // redirected to welcome page on successful login
        public IActionResult Welcome()
        {

            return View();
    }
       

        //new user registration
        [HttpPost("Create")]
        public IActionResult Create([FromForm] Authentication model)
        {
            System.Data.DataSet Response = new System.Data.DataSet();
            string response = TestApplication.DAL.Register(model);
            if (response == "success")
            {
                ViewBag.Message = "User Registered successfully";
                 return View("Register");
              
            }
            else
            {
                ViewBag.Message = response;
                return View("Register");

              

            }


        }


      
      
        //check token validity for use after login
        public bool IsTokenValid(string token)
        {
            Authentication a = new Authentication();

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Invalid Token");

            TokenValidationParameters tokenValidationParameters =
            new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience=false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(a.SecretKey)),
            };
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //generationg token on successfull login
        private string setupJWT(Authentication model)
        {
            if (model == null || model.Claims == null || model.Claims.Length == 0)
                throw new ArgumentException("Arguments to create token are not valid.");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(model.Claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(model.validity)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model.SecretKey)), model.SecurityAlgorithm)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;


        }

        //claiming the token for the user who has logged in
        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            Authentication a = new Authentication();
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters =
                  new TokenValidationParameters()
                  {
                      ValidateIssuer = false,
                      ValidateAudience=false,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(a.SecretKey)),
                  };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //creating claim container model
        private static Authentication GetModelforClaims(string username)
        {
            return new Authentication()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }
            };
        }

          //authenticate when user logs in
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromForm] Authentication model)
        {
            try
            {
                System.Data.DataSet Response = new System.Data.DataSet();
                string response = TestApplication.DAL.Login(model);
                if (response == "success")
                {
                    Authentication auth = GetModelforClaims(model.Username);
                    string tokenString = setupJWT(auth);

                    if (!IsTokenValid(tokenString))
                    {
                        ViewBag.Validuser = "Invalid user";
                       
                       return View("Login");
                    }
                    else
                    {
                        List<Claim> claims = GetTokenClaims(tokenString).ToList();

                        ///to validate token username should be equal to the below claim value
                        if (model.Username == (claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value))
                        {
                            ViewBag.Validuser = "User Valid";
                            return View("Login");
                        }
                        else

                        {
                            ViewBag.Validuser = "Invalid user";
                            return View("Login");

                        }
                    }

                }

                else
                {
                    ViewBag.Validuser = "Invalid user";
          
                    return View("Login");
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                ViewBag.Validuser = "Invalid user";
                return View("Login");

            }

        }
    }
}
