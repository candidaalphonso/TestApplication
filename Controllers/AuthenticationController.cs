using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Models;

namespace TestApplication.Controllers
{
    public class AuthenticationController : Controller
    {
       
        public IActionResult Index()
        {
            
            return View();
        }
       
        public IActionResult Register()
        {
          
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }


        [HttpPost("Create")]
        public IActionResult Create([FromForm] Authentication model)
        {
            System.Data.DataSet Response = new System.Data.DataSet();
            string response = TestApplication.DAL.Register(model);
            if (response == "success")
            {
              
                return Redirect("Home");
            }
            else
            {
               
                return View();
            }

                //return Ok(response);
                //return View();
                // return Redirect("Home");
            }
    }
}
