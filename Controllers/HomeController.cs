using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using spanApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace spanApp.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly spanDbContext context;
        List<Podaci> Podaci = new List<Podaci>();
        public HomeController(IWebHostEnvironment _environment, spanDbContext context)
        {
            Environment = _environment;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult saveToDb(IFormFile postedFile)
        {

            var tempDataStr = TempData["ListOfPodaci"] as string;
            Podaci = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Podaci>>(tempDataStr);

            for (int i = 0; i < Podaci.Count; i++)
            {
                context.Database.ExecuteSqlRaw("EXEC dbo.InsertData @first_name = {0}, @last_name = {1}, @city = {2}, @zip_code = {3}, @phone_number = {4}",
                        Podaci[i].FirstName, Podaci[i].LastName, Podaci[i].City, Podaci[i].ZipCode, Podaci[i].PhoneNumber);

                context.SaveChanges();
            }

            return Redirect("/Home/List");
        }

        [HttpPost]
        public IActionResult Index(IFormFile postedFile)
        {
            if (postedFile != null)
            {
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                string csvData = System.IO.File.ReadAllText(filePath);

                var list = new List<Podaci>();
                
                foreach (string row in csvData.Split('\n'))
                {
                    string[] stringArray = row.Split(';');

                    Podaci podatak = new Podaci
                    {
                        FirstName = stringArray[0],
                        LastName = stringArray[1],
                        ZipCode = stringArray[2],
                        City = stringArray[3],
                        PhoneNumber = stringArray[4]
                    };

                    list.Add(podatak);
                }

                var podaciString = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                TempData["ListOfPodaci"] = podaciString;

                return View(list);
            }

            return View();
        }
    }

}


