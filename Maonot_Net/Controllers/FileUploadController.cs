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
        //return the view Index_2 for uplode files
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
        //get a list of file that the user upload and save them
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
        // return a list of fiels that the user uplouad
        public async Task<IActionResult> SeeFiles(int student)
        {
            string Aut = HttpContext.Session.GetString("Aut");
            ViewBag.Aut = Aut;
            var userId = HttpContext.Session.GetString("User");
            string studentId = "";
            if (student == null)
            {
                studentId = userId;
            }
            else
            {
                studentId = student.ToString();
            }
             
            var functions = new functions();
            var u = await _context.Registrations.SingleOrDefaultAsync(m => m.StundetId.ToString().Equals(studentId));

            if (functions.Comper(new DateTime(2019, 7, 30)))
            {
                if (Aut.Equals("2") || userId.Equals(u.StundetId.ToString()))
                {
                    if (!Directory.Exists(Path.Combine(
                                Directory.GetCurrentDirectory(), $"wwwroot/{studentId}")))
                    {
                        return RedirectToAction("NoFiles", "Home");
                    }
                    string[] filePaths = Directory.GetFiles(@"wwwroot\" + studentId);
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
        //delete a file 
        public IActionResult Delete(string File)
        {
            var userId = HttpContext.Session.GetString("User");
            string filePath = @"wwwroot\" + File;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return RedirectToAction("SeeFiles",new { student= userId });
        }

    }
}