using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shoposphere.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UserEvaluation.Admin.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using UserEvaluation.Services.Interfaces;
using System.Security.Claims;

using UserEvaluation.Models;

namespace UserEvaluation.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<User> _userRepository;
        public HomeController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
           
        

            return View();
        }

    

    }
}
