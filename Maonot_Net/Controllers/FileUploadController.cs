using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Maonot_Net.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maonot_Net.Controllers
{
    
    public class FileUploadController : Controller
    {

        private readonly MaonotNetContext _context;

        public FileUploadController(MaonotNetContext context)
        {

            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        //שיקסלת רשימה של אי כםרצ ויעבור על הרשימה ויעשה את מה שצריך
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var userId = "308242122";

            if (!Directory.Exists(Path.Combine(
                        Directory.GetCurrentDirectory(), $"wwwroot/{userId}")))
            {
                Directory.CreateDirectory(Path.Combine(
                        Directory.GetCurrentDirectory(), $"wwwroot/{userId}"));
            }
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), $"wwwroot/{userId}",
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("Files");
        }

        public IActionResult Index_2()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            if (Aut.Equals("8")|| Aut.Equals("7"))
            {
                return View();
            }
            return RedirectToAction("NotAut", "Home");
        }
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            foreach (var file in files) {
                if (file == null || file.Length == 0)
                    return Content("file not selected");

                var userId = HttpContext.Session.GetString("User");

                if (!Directory.Exists(Path.Combine(
                            Directory.GetCurrentDirectory(), $"wwwroot/{userId}")))
                {
                    Directory.CreateDirectory(Path.Combine(
                            Directory.GetCurrentDirectory(), $"wwwroot/{userId}"));
                }
                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), $"wwwroot/{userId}",
                            file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            TempData["msg"] = "<script>alert('ההרשמה הושלמה בהצלחה');</script>";
            return  RedirectToAction("Wellcome", "Home");
        }

        public async Task<IActionResult> SeeFiles()
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            var userId = HttpContext.Session.GetString("User");
            var functions = new functions();
            var u = await _context.Registrations.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(userId));

            if (functions.Comper(new DateTime(2019, 7, 30)))
            {
                if (Aut.Equals("2") || userId.Equals(u.StundetId.ToString()))
                {
                    if (!Directory.Exists(Path.Combine(
                                Directory.GetCurrentDirectory(), $"wwwroot/{userId}")))
                    {
                        return NotFound();
                    }
                    string[] filePaths = Directory.GetFiles(@"wwwroot\" + userId);
                    List<string> list = new List<string> { };
                    foreach (var file in filePaths)
                    {
                        string s = file.Substring(8);


                        list.Add(s);
                    }
                    ViewBag.url = list;
                    return View();
                }
            }
            return RedirectToAction("NotAut", "Home");
        }

        public IActionResult Delete(string File)
        {
            string filePath = @"wwwroot\" + File;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return RedirectToAction("SeeFiles");
        }

    }
}