﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserEvaluation.Admin.Models;
using Shoposphere.Data.Entities;
using UserEvaluation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserEvaluation.Controllers;

namespace UserEvaluation.Admin.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IRepository<User> _userRepository;
        public AuthController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

       

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Email = model.Email,
                Password = model.Password,
                BirthDate = model.BirthDate,
                CreatedById = -1,
                CreatedDate = DateTime.Now,
                Phone=model.Phone,
                RoleId = 1,
                
            };

            var result = _userRepository.Add(user);
            if (result)
            {
                // kayıttan sonra oturun açmak için login'e gelir.
                TempData["Message"] = "You have successfully Registered";
                return RedirectToAction("Login");
            }

            TempData["Message"] = "Registration failed!";
            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // user dbde var mı?
            var user = _userRepository.Get(x => x.Email == model.Email && x.Password == model.Password);

            if (user != null)
            {
                // varsa authentication yapılacak

                // Authentication/Authorization için 
                // 1. startup.cs'de auth mekanizmasını eklemek
                // 2. login içinde authenticated yapmak

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.RoleId.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
               
                HttpContext.SignInAsync(claimsPrincipal);

                if (user.RoleId==1)
                {
                    return RedirectToAction("Add", "Evaluation");
                }

                else
                    return RedirectToAction("InProcess","Evaluation");
                
            }

            ViewBag.Message = "Kullanıcı adınızı veya şifrenizi kontrol ediniz.";
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("login");
        }
    }
}

