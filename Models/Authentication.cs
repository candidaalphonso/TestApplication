using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
namespace TestApplication.Models
{
    public class Authentication
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Token { get; set; }
        public int validity { get; set; } = 120; // in minutes
        public string SecretKey { get; set; } = "askjqr092$(*_5522GHFKjhf34_^&%38kjdaskhdbe467a5f1s4d32f4s54d38472384u2hg3476^&&^$"; 
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

        public Claim[] Claims { get; set; }
    }

   
}
