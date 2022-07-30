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
using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Firebase.Auth;
using System.Threading;
using Firebase.Storage;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using RestSharp.Authenticators.OAuth;
using System.Web;
using System.Security.Cryptography.Xml;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using System.Runtime.Serialization.Json;

namespace Lifesum.Controllers
{
    public class FoodController : Controller
    {
        public static string ApiKey = "AIzaSyDI84FBWnciXiaVq78PXmeGD-Vt6rj6WKs";
        public static string Bucket = "lifesum-99433.appspot.com";
        public static string AuthEmail = "shams@gmail.com";
        public static string AuthPassword = "shams12";

        FirestoreDb db;

        public async Task Go()
        {
            HttpClient client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes("eef0b4de26264a279cfdda9f0d2f7caf:f3dce2f595b741c0951ad63ffbca85b4");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var values = new Dictionary<string, string>
            {
                { "scope", "basic" },
                { "grant_type", "client_credentials" }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://oauth.fatsecret.com/connect/token", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(responseString));
            var data = (RootObject)serializer.ReadObject(ms);

            //Get Response

            var http = new HttpClient();

            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.access_token);

            var valuesRequest = new Dictionary<string, string>
            {
                { "application", "json" },
            };

            var contentRequest = new FormUrlEncodedContent(valuesRequest);

            var responseRequest = await http.GetAsync("https://platform.fatsecret.com/rest/server.api?&food_id=33691&method=food.get.v2&format=json");
            responseRequest.EnsureSuccessStatusCode();

            //Parameters: method=foods.search&search_expression=toast&format=json // How can I call Api 2.0 with these parameters?
            var responseRequestString = await responseRequest.Content.ReadAsStringAsync();
        }


        public async Task<IActionResult> Index()
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
                    ViewBag.Data = listaCategory;
                }
            }
            return View();

        }

        public async Task<IActionResult> GetServingsList(string id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentreference = db.Collection("Categories").Document(id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<Category> listaCategory = new List<Category>();


            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                Category std = JsonConvert.DeserializeObject<Category>(json);
                std.CategoryId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                listaCategory.Add(std);

                ViewBag.data = listaCategory; /*new SelectList(listaCategory, "CategoryId", "servings");*/

            }
            return PartialView("DisplayServe");

        }


        public async Task<IActionResult> GetServingList(string id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentreference = db.Collection("Categories").Document(id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<Category> listaCategory = new List<Category>();


            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                Category std = JsonConvert.DeserializeObject<Category>(json);
                std.CategoryId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                listaCategory.Add(std);

                ViewBag.data = listaCategory; /*new SelectList(listaCategory, "CategoryId", "servings");*/

            }
            return PartialView("DisplayServings");

        }

        public async Task<IActionResult> GetSerList(string id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentreference = db.Collection("Foods").Document(id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<CreateFood> listaFood = new List<CreateFood>();


            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                CreateFood std = JsonConvert.DeserializeObject<CreateFood>(json);
                std.FoodId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                listaFood.Add(std);

                ViewBag.data = listaFood;

                Query docref = db.Collection("Categories");
                QuerySnapshot snap = await docref.GetSnapshotAsync();
                List<Category> listaCategory = new List<Category>();
                foreach (DocumentSnapshot docSnapshot in snap.Documents)
                {
                    if (docSnapshot.Exists)
                    {
                        Dictionary<string, object> cat1 = docSnapshot.ToDictionary();
                        string json1 = JsonConvert.SerializeObject(cat1);
                        Category std1 = JsonConvert.DeserializeObject<Category>(json1);
                        std1.CategoryId = docSnapshot.Id;
                        //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                        listaCategory.Add(std1);

                        ViewBag.cate = listaCategory;
                    }
                }
            }
            return PartialView("DisplayCatServing");

        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(CreateFood obj, Dictionary ob, string customServing, IFormFile image)
        {
            db = FirestoreDb.Create("lifesum-99433");
            CollectionReference collectionReference = db.Collection("Foods");

            Query docref = db.Collection("Foods");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<CreateFoodDto> listaFood = new List<CreateFoodDto>();
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    CreateFoodDto std = JsonConvert.DeserializeObject<CreateFoodDto>(json);
                    std.FoodId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaFood.Add(std);

                    foreach (var item in listaFood)
                    {
                        if (obj.title == item.title)
                        {
                            TempData["Msg"] = "Food exist!";
                            return RedirectToAction(nameof(GetFood));
                        }
                    }
                }
            }

            if (image == null || image.Length == 0)
                return Content("file not selected");

            var paths = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

            using (var stream = new FileStream(paths, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            string filename = image.FileName;

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
                obj.Image = link;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex.Message);
            }


            List<string> titlesSearch = new List<string>();
            titlesSearch = Test(obj.title);

            List<string> brandSearch = new List<string>();
            brandSearch = Test(obj.brand);

            //Ye Open kerna hay
            //if (ob.servingName == "Create Custom Serving")
            //{
            //    ob.servingName = customServing;
            //}
            List<Dictionary> serving = new List<Dictionary>();
            serving.Add(ob);


            obj.titlesSearch = titlesSearch;
            obj.brandSearch = brandSearch;
            obj.serving = serving;
            var Id = collectionReference.Document().Id;
            obj.Id = Id;
            await collectionReference.Document(Id).SetAsync(obj);
            return RedirectToAction(nameof(GetFood));

        }



        public List<string> Test(string givenString)
        {
            List<string> result = new List<string>();

            string modifiedString = givenString.ToLower().Replace(" ", "");
            char[] sd = modifiedString.ToCharArray();

            List<string> givenList = new List<string>();
            foreach (char item in sd)
            {
                givenList.Add(item.ToString());
            }
            int cnt = givenList.Count;
            for (int i = 0; i < givenList.Count; i++)
            {

                List<string> subList = givenList.GetRange(i, cnt);
                for (int j = 1; j <= subList.Count; j++)
                {
                    List<string> subList2 = subList.GetRange(0, j);
                    string keyWord = "";

                    foreach (var item in subList2)
                    {
                        keyWord += item;
                        result.Add(keyWord);
                    }
                }
                cnt--;
            }
            var sdf = result.Distinct().ToList();
            return sdf;
        }


        public async Task<IActionResult> Serving()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            Query docref = db.Collection("Foods");
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
                    ViewBag.Data = listaCategory;
                }
            }

            Query docref1 = db.Collection("Categories");
            QuerySnapshot snap1 = await docref1.GetSnapshotAsync();
            List<CatDto> listaCategory1 = new List<CatDto>();

            foreach (DocumentSnapshot documentSnapshot1 in snap1.Documents)
            {
                if (documentSnapshot1.Exists)
                {
                    Dictionary<string, object> cat1 = documentSnapshot1.ToDictionary();
                    string json1 = JsonConvert.SerializeObject(cat1);
                    CatDto std1 = JsonConvert.DeserializeObject<CatDto>(json1);
                    std1.CatId = documentSnapshot1.Id;
                    //Category stud = documentSnapshot.ConvertTo<Category>();

                    listaCategory1.Add(std1);

                    ViewBag.data1 = listaCategory1; /*new SelectList(listaCategory, "CategoryId", "servings");*/

                }
            }
            return View();
            //return RedirectToAction("GetServings");

        }
        [HttpPost]
        public async Task<IActionResult> Serving(Dictionary obj, string customServing, string FoodId)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference washingtonRef = db.Collection("Foods").Document(FoodId);
            DocumentSnapshot documentSnapshot = await washingtonRef.GetSnapshotAsync();
            List<CreateFood> listaFood = new List<CreateFood>();


            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                CreateFood std = JsonConvert.DeserializeObject<CreateFood>(json);
                std.FoodId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                listaFood.Add(std);

                foreach (var items in listaFood)
                {
                    if (items.serving != null)
                    {
                        //Ye Open kerna hay
                        //foreach (var item in items.serving)
                        //{
                        //    if (item.servingName == obj.servingName || item.servingName == customServing)
                        //    {
                        //        TempData["Msg"] = "Serving Already Exist For This Food";
                        //        return RedirectToAction("Serving");
                        //    }
                        //}
                    }

                }

            }

            //Ye open kerna hay
            //if (obj.servingName == "Create Custom Serving")
            //{
            //    obj.servingName = customServing;
            //}

            await washingtonRef.UpdateAsync("serving", FieldValue.ArrayUnion(obj));

            TempData["Msg"] = "Serving Added Successfully!";
            return RedirectToAction("Serving");

        }

        public async Task<IActionResult> GetFood()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            var uId = HttpContext.Session.GetString("userId");
            if (uId != null)
            {
                Query docref = db.Collection("Foods");
                QuerySnapshot snap = await docref.GetSnapshotAsync();
                List<CreateFoodDto> listaFood = new List<CreateFoodDto>();
                foreach (DocumentSnapshot documentSnapshot in snap.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(cat);
                        CreateFoodDto std = JsonConvert.DeserializeObject<CreateFoodDto>(json);
                        std.FoodId = documentSnapshot.Id;
                        //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                        listaFood.Add(std);
                    }
                }
                return View(listaFood);
            }

            return RedirectToAction("Login", "Login");

        }
        public async Task<IActionResult> GetServings()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            Query docref = db.Collection("Foods");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<ServingDto> listaCategory = new List<ServingDto>();
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    ServingDto std = JsonConvert.DeserializeObject<ServingDto>(json);
                    std.FoodId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();


                    listaCategory.Add(std);
                    ViewBag.Data = listaCategory;
                }
            }
            return View(null);

        }
        [HttpPost]
        public async Task<IActionResult> GetServings(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentreference = db.Collection("Foods").Document(Id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<ServingNewDto> listaServing = new List<ServingNewDto>();
            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                ServingNewDto std = JsonConvert.DeserializeObject<ServingNewDto>(json);

                //ServingNewDto[] orderList = new JavaScriptSerializer().Deserialize<ServingNewDto[]>(json);

                std.FoodId = documentSnapshot.Id;
                //listaCategory.Add(std);

                listaServing.Add(std);
            }
            return PartialView("~/Views/Food/_GetServings.cshtml", listaServing);

        }
        public async Task<IActionResult> EditFood(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentreference = db.Collection("Foods").Document(Id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<CreateFood> listaCategory = new List<CreateFood>();

            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                CreateFood std = JsonConvert.DeserializeObject<CreateFood>(json);
                std.FoodId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                //List<string> str = new List<string>();
                //List<string> str1 = new List<string>();

                //var sd = string.Join(',', std.servings);
                //var sd1 = string.Join(',', std.subcategories);

                //str.Add(sd);
                //str1.Add(sd1);

                //std.servings = str;
                //std.subcategories = str1;

                listaCategory.Add(std);


                Query docref1 = db.Collection("Foods");
                QuerySnapshot snap1 = await docref1.GetSnapshotAsync();
                List<Category> listaCategory1 = new List<Category>();
                foreach (DocumentSnapshot documentSnapshot1 in snap1.Documents)
                {
                    if (documentSnapshot1.Exists)
                    {
                        Dictionary<string, object> cat1 = documentSnapshot1.ToDictionary();
                        string json1 = JsonConvert.SerializeObject(cat1);
                        Category std1 = JsonConvert.DeserializeObject<Category>(json1);
                        std1.CategoryId = documentSnapshot1.Id;
                        //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();


                        listaCategory1.Add(std1);
                        ViewBag.Data = listaCategory1;
                    }
                }
            }
            return View(listaCategory);

        }
        [HttpPost]
        public async Task<IActionResult> EditFood(EditFood dto, IFormFile Img, string image)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentReference = db.Collection("Foods").Document(dto.FoodId);

            if (Img == null || Img.Length == 0)
            {
                dto.Image = image;
            }
            else
            {
                var paths = Path.Combine(
                           Directory.GetCurrentDirectory(), "wwwroot/images", Img.FileName);

                using (var stream = new FileStream(paths, FileMode.Create))
                {
                    await Img.CopyToAsync(stream);
                }

                string filename = Img.FileName;

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
                    dto.Image = link;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception was thrown: {0}", ex.Message);
                }
            }
           
            await documentReference.SetAsync(dto, SetOptions.MergeAll);
            return RedirectToAction("GetFood");

        }
        public async Task<IActionResult> Delete(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("Foods").Document(Id);
            await documentReference.DeleteAsync();
            TempData["Msg"] = "Food Deleted!";
            return RedirectToAction("GetFood");

        }
        public async Task<IActionResult> DeleteServing(ServingArrayDto model, string Id, string Indx)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference washingtonRef = db.Collection("Foods").Document(Id);

            DocumentSnapshot documentSnapshot = await washingtonRef.GetSnapshotAsync();
            List<ServingArrayDto> listaServing = new List<ServingArrayDto>();
            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                ServingArrayDto std = JsonConvert.DeserializeObject<ServingArrayDto>(json);


                //std.FoodId = documentSnapshot.Id;
                listaServing.Add(std);

                ViewBag.arrFood = listaServing;

                List<Dictionary> servingsArray = new List<Dictionary>();

                foreach (var item in ViewBag.arrFood)
                {
                    foreach (var i in item.serving)
                    {
                        if (i.servingName != Indx)
                        {
                            servingsArray.Add(i);
                            model.serving = servingsArray;
                        }
                    }
                }

            }

            await washingtonRef.SetAsync(model, SetOptions.MergeAll);
            TempData["Mesg"] = "Serving Deleted!";
            return RedirectToAction("GetServings");

        }
    }
}
