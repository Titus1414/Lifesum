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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Firebase.Storage;
using System.Threading;
using Firebase.Auth;
using Nancy.Json;
using Microsoft.AspNetCore.Authorization;

namespace Lifesum.Controllers
{
    public class PlansController : Controller
    {
        public static string ApiKey = "AIzaSyDI84FBWnciXiaVq78PXmeGD-Vt6rj6WKs";
        public static string Bucket = "lifesum-99433.appspot.com";
        public static string AuthEmail = "shams@gmail.com";
        public static string AuthPassword = "shams12";

        FirestoreDb db;
        public IActionResult Index()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            return View();
        }

        public async Task<IActionResult> GetPlans()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            Query query = db.Collection("Plans");
            QuerySnapshot querySnapsht = await query.GetSnapshotAsync();
            List<Plans> listaPlan = new List<Plans>();
            foreach (DocumentSnapshot Snapshot in querySnapsht.Documents)
            {
                if (Snapshot.Exists)
                {
                    Dictionary<string, object> cat = Snapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    Plans std = JsonConvert.DeserializeObject<Plans>(json);
                    std.plansId = Snapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaPlan.Add(std);
                    ViewBag.high = listaPlan;
                }
            }
            return View(listaPlan);
        }


        [HttpGet]

        public async Task<IActionResult> Edit(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentreference = db.Collection("Plans").Document(Id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<Plans> listaPlans = new List<Plans>();

            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                Plans std = JsonConvert.DeserializeObject<Plans>(json);
                std.plansId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                List<string> str = new List<string>();
                List<string> str1 = new List<string>();
                List<string> str2 = new List<string>();

                var sd = string.Join(',', std.dos);
                var sd1 = string.Join(',', std.donts);
                var sd2 = string.Join(',', std.features);

                str.Add(sd);
                str1.Add(sd1);
                str2.Add(sd2);

                std.dos = str;
                std.donts = str1;
                std.features = str2;

                listaPlans.Add(std);
            }

            Query docref = db.Collection("Recipes");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<CreateRecipe> listaRecipe = new List<CreateRecipe>();
            foreach (DocumentSnapshot documentSnap in snap.Documents)
            {
                if (documentSnap.Exists)
                {
                    Dictionary<string, object> cat = documentSnap.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    CreateRecipe std = JsonConvert.DeserializeObject<CreateRecipe>(json);
                    std.recipeId = documentSnap.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaRecipe.Add(std);
                    ViewBag.Recipes = listaRecipe;

                }
            }

            return View(listaPlans);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Plans model, IFormFile coverImage, string coveImage)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("Plans").Document(model.plansId);

            if (model.carbs + model.fat + model.protein != 100)
            {
                TempData["Msg"] = "Sum of Protein, carbs and fat must be equal to 100";
                return RedirectToAction(nameof(GetPlans));
            }
            else
            {
                List<string> dos = new List<string>();
                foreach (var item in model.dos)
                {
                    if (item != null)
                    {
                        List<string> TagIds = item.Split(',').ToList();
                        dos = TagIds;
                    }
                }
                model.dos = dos;

                List<string> donts = new List<string>();
                foreach (var item in model.donts)
                {
                    if (item != null)
                    {
                        List<string> TagIds = item.Split(',').ToList();
                        donts = TagIds;
                    }
                }
                model.donts = donts;

                List<string> features = new List<string>();
                foreach (var item in model.features)
                {
                    if (item != null)
                    {
                        List<string> TagIds = item.Split(',').ToList();
                        features = TagIds;
                    }
                }
                model.features = features;


                if (coverImage == null || coverImage.Length == 0)
                {
                    model.coverImage = coveImage;
                }
                else
                {
                    var paths = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/images", coverImage.FileName);

                    using (var stream = new FileStream(paths, FileMode.Create))
                    {
                        await coverImage.CopyToAsync(stream);
                        stream.Flush();
                        stream.Close();
                    }

                    string filename = coverImage.FileName;

                    FileStream str;
                    str = new FileStream(paths, FileMode.Open);

                    //await Task.Run(() => Upload(str, filename));

                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                    var cancellation = new CancellationTokenSource();

                    var task = new FirebaseStorage(
                        Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true
                        })
                        .Child("images")
                        .Child("1zXAnPSVBvcEen02eQ3NAZlAgik1")
                        .Child(filename)
                        .PutAsync(str, cancellation.Token);

                    try
                    {
                        string link = await task;
                        model.coverImage = link;
                        str.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception was thrown: {0}", ex.Message);
                    }
                }

                await documentReference.SetAsync(model, SetOptions.MergeAll);
                return RedirectToAction(nameof(GetPlans));
            }
        }


        public async Task<IActionResult> PlanRecipes()
        {
            db = FirestoreDb.Create("lifesum-99433");

            Query query = db.Collection("Plans");
            QuerySnapshot querySnapsht = await query.GetSnapshotAsync();
            List<Plans> listaPlan = new List<Plans>();
            foreach (DocumentSnapshot Snapshot in querySnapsht.Documents)
            {
                if (Snapshot.Exists)
                {
                    Dictionary<string, object> cat = Snapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    Plans std = JsonConvert.DeserializeObject<Plans>(json);
                    std.plansId = Snapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaPlan.Add(std);
                    ViewBag.Plans = listaPlan;
                }
            }

            Query docref = db.Collection("Recipes");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<CreateRecipe> listaRecipe = new List<CreateRecipe>();
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    CreateRecipe std = JsonConvert.DeserializeObject<CreateRecipe>(json);
                    std.recipeId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaRecipe.Add(std);
                    ViewBag.Recipes = listaRecipe;

                }
            }



            return View(listaRecipe);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipetoPlan(PlanRecipes model, string plansId)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentReference = db.Collection("Plans").Document(plansId);
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();
            List<PlanRecipes> listaPlans = new List<PlanRecipes>();

            List<string> recipes = new List<string>();
            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                PlanRecipes std = JsonConvert.DeserializeObject<PlanRecipes>(json);
                std.plansId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                listaPlans.Add(std);

                foreach(var lists in listaPlans)
                {
                    if(lists.recipes!=null)
                    {
                        foreach (var list in lists.recipes)
                        {
                            foreach(var r in model.recipes)
                            {
                                if (list == r)
                                {
                                    TempData["Msg"] = "Recipe Already Exist!";
                                    return RedirectToAction("PlanRecipes");
                                }
                            }
                            
                        }
                    }
                    
                }

                    foreach (var items in listaPlans)
                    {
                        if (items.recipes != null)
                        {
                            foreach (var item in items.recipes)
                            {
                                model.recipes.Add(item);
                            }
                        }
                       
                    }
                
            }
            await documentReference.SetAsync(model, SetOptions.MergeAll);
            return RedirectToAction(nameof(GetPlans));

        }

        [HttpGet]
        public async Task<IActionResult> EditRecipe(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentreference = db.Collection("Plans").Document(Id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<PlanRecipes> listaPlans = new List<PlanRecipes>();

            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                PlanRecipes std = JsonConvert.DeserializeObject<PlanRecipes>(json);
                std.plansId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                listaPlans.Add(std);
            }

            Query docref = db.Collection("Recipes");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<CreateRecipe> listaRecipe = new List<CreateRecipe>();
            foreach (DocumentSnapshot documentSnap in snap.Documents)
            {
                if (documentSnap.Exists)
                {
                    Dictionary<string, object> cat = documentSnap.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    CreateRecipe std = JsonConvert.DeserializeObject<CreateRecipe>(json);
                    std.recipeId = documentSnap.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaRecipe.Add(std);
                    ViewBag.Recipes = listaRecipe;

                }
            }

            return View(listaPlans);
        }

        [HttpPost]

        public async Task<IActionResult> EditRecipe(PlanRecipes model, string plansId)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("Plans").Document(plansId);

            await documentReference.SetAsync(model, SetOptions.MergeAll);
            return RedirectToAction(nameof(GetPlans));
          
        }


    }
}



