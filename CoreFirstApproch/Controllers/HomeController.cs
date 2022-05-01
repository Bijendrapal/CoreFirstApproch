using CoreFirstApproch.DB_Context;
using CoreFirstApproch.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreFirstApproch.Controllers
{


   [Authorize]
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {


            List<Employee> model = new List<Employee>();
            StuContext entites = new StuContext();
            var tbl = entites.Mytable1s;
            foreach (var item in tbl)
            {
                model.Add(new Employee
                {
                   Id=item.Id,
                   Name=item.Name,
                   Email=item.Email,
                    Department = item.Department,
                   Salary =item.Salary
                  

                });
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
        [HttpPost]
        public IActionResult About( Mytable1 model)
        {
            StuContext entites = new StuContext();
            Mytable1 tbl = new Mytable1();
            tbl.Id = model.Id;
            tbl.Name = model.Name;
            tbl.Email = model.  Email;
            tbl.Department = model. Department;
            tbl.Salary = model.Salary;
            if(model.Id==0)
            {
                entites.Mytable1s.Add(tbl);
                entites.SaveChanges();
            }
            else
            {
                entites.Entry(tbl).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                entites.SaveChanges();
            }


            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            Employee model = new Employee();
            StuContext enties = new StuContext();
            var tabl = enties.Mytable1s.Where(m => m.Id == id).FirstOrDefault();
            model.Id = tabl.Id;
            model.Name = tabl.Name;
            model.Email = tabl.Email;
            model.Department = tabl.Department;
            model.Salary = tabl.Salary;

            return View("About", model);
        }


        public IActionResult Delete(int id)
        {
            StuContext enteis = new StuContext();
            var del = enteis.Mytable1s.Where(m => m.Id == id).FirstOrDefault();
            enteis.Mytable1s.Remove(del);
            enteis.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [AllowAnonymous]

        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(Userlog mod)
        {
            StuContext enties = new StuContext();
            var res = enties.UserLog1s.Where(m => m.Email == mod.Email).FirstOrDefault();
            if (res==null)
            {
                TempData["Worng"] = "Email is Not Found";
            }
            else
            {
                if (res.Email==mod.Email && res.Password==mod.Password)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, res.Name), new Claim(ClaimTypes.Email, res.Email) };
                    var identiy = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identiy));
                    HttpContext.Session.SetString("Name", res.Name);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.DB = "Wromg Email id or Password";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }


        public IActionResult SigOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
