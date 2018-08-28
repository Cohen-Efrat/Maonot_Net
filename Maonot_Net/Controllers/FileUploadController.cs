using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maonot_Net.Controllers
{
    public class FileUploadController : Controller
    {
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
            return View();
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

        public IActionResult SeeFiles()
        {
            var userId = HttpContext.Session.GetString("User");
            //var userId = "308242122";
            string[] filePaths = Directory.GetFiles(@"wwwroot\"+ userId);
            List<string> list = new List<string> { };
            foreach (var file in filePaths)
            {
                string s = file.Substring(8);


                list.Add(s);
            }
            ViewBag.url = list;
            return View();
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