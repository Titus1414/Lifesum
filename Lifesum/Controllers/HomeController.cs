using Lifesum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Lifesum.Controllers
{
    public class HomeController : Controller
    {
        FirestoreDb db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category obj)
        {
            db = FirestoreDb.Create("lifesum-99433");
            CollectionReference collectionReference = db.Collection("Categories");

            Query docref = db.Collection("Categories");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<Category> listaCategory = new List<Category>();
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    Category std = JsonConvert.DeserializeObject<Category>(json);
                    std.CategoryId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaCategory.Add(std);

                    foreach(var item in listaCategory)
                    {
                        if(obj.title == item.title)
                        {
                            TempData["Msg"] = "Category exist!";
                            return RedirectToAction(nameof(GetCategories));
                        }
                    }
                }
            }



            List<string> servings = new List<string>();
            List<string>  subcategories = new List<string>();
            foreach (var item in obj.servings)
            {
                if (item != null)
                {
                    List<string> TagIds = item.Split(',').ToList();
                    servings = TagIds;
                }
            }
            obj.servings = servings;
            foreach (var item1 in obj.subcategories)
            {
                if (item1 != null)
                {
                    List<string> TagIds1 = item1.Split(',').ToList();
                    subcategories = TagIds1;
                }
            }
            obj.subcategories = subcategories;

            await collectionReference.AddAsync(obj);
            return RedirectToAction(nameof(GetCategories));
        }

        public async Task<IActionResult> GetCategories()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            Query docref = db.Collection("Categories");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<Category> listaCategory = new List<Category>();
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    Category std = JsonConvert.DeserializeObject<Category>(json);
                    std.CategoryId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();
                    
                    listaCategory.Add(std);
                }
            }

            return View(listaCategory);

        }

        [HttpGet]

        public async Task<IActionResult> Edit(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentreference = db.Collection("Categories").Document(Id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<Category> listaCategory = new List<Category>();

            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                Category std = JsonConvert.DeserializeObject<Category>(json);
                std.CategoryId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                List<string> str = new List<string>();
                List<string> str1 = new List<string>();
                
                var sd = string.Join(',', std.servings);
                var sd1 = string.Join(',', std.subcategories);

                str.Add(sd);
                str1.Add(sd1);

                std.servings = str;
                std.subcategories = str1;

                listaCategory.Add(std);
            }
            return View(listaCategory);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Category obj)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentReference = db.Collection("Categories").Document(obj.CategoryId);

            //Query docref = db.Collection("Categories");
            //QuerySnapshot snap = await docref.GetSnapshotAsync();
            //List<Category> listaCategory = new List<Category>();
            //foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            //{
            //    if (documentSnapshot.Exists)
            //    {
            //        Dictionary<string, object> cat = documentSnapshot.ToDictionary();
            //        string json = JsonConvert.SerializeObject(cat);
            //        Category std = JsonConvert.DeserializeObject<Category>(json);
            //        std.CategoryId = documentSnapshot.Id;
            //        //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

            //        listaCategory.Add(std);

            //        foreach (var item in listaCategory)
            //        {
            //            if (obj.title == item.title)
            //            {
            //                //TempData["Msg"] = "Edit Successfully!";
            //                return RedirectToAction(nameof(GetCategories));
            //            }
            //        }
            //    }
            //}

            List<string> servings = new List<string>();
            List<string> subcategories = new List<string>();
            foreach (var item in obj.servings)
            {
                if (item != null)
                {
                    List<string> TagIds = item.Split(',').ToList();
                    servings = TagIds;
                }
            }
            obj.servings = servings;
            foreach (var item1 in obj.subcategories)
            {
                if (item1 != null)
                {
                    List<string> TagIds1 = item1.Split(',').ToList();
                    subcategories = TagIds1;
                }
            }
            obj.subcategories = subcategories;

            await documentReference.SetAsync(obj, SetOptions.MergeAll);
            return RedirectToAction(nameof(GetCategories));
        }

        public async Task<IActionResult> Delete(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("Categories").Document(Id);
            await documentReference.DeleteAsync();
            TempData["Msg"] = "Category Deleted!";
            return RedirectToAction(nameof(GetCategories));
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
