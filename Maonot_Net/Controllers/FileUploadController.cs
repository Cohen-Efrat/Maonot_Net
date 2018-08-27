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
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");


            Directory.CreateDirectory(Path.Combine(
                        Directory.GetCurrentDirectory(), "try1",
                        file.FileName)); 

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "try1",
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("Files");
        }


        /* public async Task<IActionResult> Download(string filename)
         {
             if (filename == null)
                 return Content("filename not present");

             var path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot", filename);

             var memory = new MemoryStream();
             using (var stream = new FileStream(path, FileMode.Open))
             {
                 await stream.CopyToAsync(memory);
             }
             memory.Position = 0;
             return File(memory, path.GetType(), Path.GetFileName(path));
         }*/



    }
}