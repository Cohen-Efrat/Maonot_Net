using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maonot_Net.Controllers
{
    public class UploadmultipleController : Controller
    {

        private IHostingEnvironment _hostingEnvironment;


        public UploadmultipleController (IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
       [HttpPost]
        public IActionResult Index(IList<IFormFile> files)
        {
            foreach (IFormFile item in files)
            {
                string filename = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                filename = this.EnsureFilename(filename);
                using(FileStream filestream = System.IO.File.Create(this.Getpath(filename)))
                {


                }
            }
            return this.Content("Success");
        }

        private string Getpath(string filename)
        {
            
            // change the upload to take session id
            string path = _hostingEnvironment.WebRootPath + "\\Upload\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + filename;
        }

        private string EnsureFilename(string filename)
        {
            //throw new NotImplementedException();
            if (filename.Contains("\\"))
            {
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            }
            return filename;
        }

        public IActionResult SeeFiles()
        {
            //< a href = "~/Files/contract.pdf" target = "_blank" > חוזה מעונות </ a >
                  string[] filePaths = Directory.GetFiles(@"wwwroot\Upload");
            List<string> list = new List<string> { };
            foreach (var file in filePaths)
            {
                string s = file.Substring(15);
            
                list.Add(s);
            }
            ViewBag.url = list;
            return View();
        }


    }
}