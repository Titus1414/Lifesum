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
using Grpc.Core;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using static Lifesum.Models.FoodServingJson;

namespace Lifesum.Controllers
{
    public class RecipeController : Controller
    {
        public static string ApiKey = "AIzaSyDI84FBWnciXiaVq78PXmeGD-Vt6rj6WKs";
        public static string Bucket = "lifesum-99433.appspot.com";
        public static string AuthEmail = "shams@gmail.com";
        public static string AuthPassword = "shams12";

        FirestoreDb db;
        public async Task<IActionResult> Index()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            foodlist.Clear();

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
                    ViewBag.Food = listaFood;
                }
            }

            Query documentref = db.Collection("Filter");
            QuerySnapshot snaps = await documentref.GetSnapshotAsync();
            List<Filter> listaFilter = new List<Filter>();
            foreach (DocumentSnapshot documentSnapshot in snaps.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> filter = documentSnapshot.ToDictionary();
                    string jsonData = JsonConvert.SerializeObject(filter);
                    Filter std = JsonConvert.DeserializeObject<Filter>(jsonData);
                    std.filterId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();


                    listaFilter.Add(std);
                    ViewBag.Filter = listaFilter;
                }
            }

            return View();
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

            }
            return PartialView("DisplayCatServing");
        }

        //foodsearch Api

        [HttpPost]
        public async Task<IActionResult> DisplaySearchFood(string foodName)
        {
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
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
            var responseRequest = await http.GetAsync("https://platform.fatsecret.com/rest/server.api?&search_expression=" + foodName + "&method=foods.search&format=json");
            responseRequest.EnsureSuccessStatusCode();

            //Parameters: method=foods.search&search_expression=toast&format=json // How can I call Api 2.0 with these parameters?
            var responseRequestString = await responseRequest.Content.ReadAsStringAsync();

            JSONMODEL.Rootobject records = JsonConvert.DeserializeObject<JSONMODEL.Rootobject>(responseRequestString);
            List<JSONMODEL.Rootobject> listaFood = new List<JSONMODEL.Rootobject>();

            listaFood.Add(records);

            ViewBag.FoodData = listaFood;

            return PartialView("DisplaySearchFood");
        }

        [HttpPost]
        public async Task<IActionResult> DisplayFoodServing(int id)
        {
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
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
            var responseRequest = await http.GetAsync("https://platform.fatsecret.com/rest/server.api?&food_id=" + id + "&method=food.get.v2&format=json");
            responseRequest.EnsureSuccessStatusCode();

            //Parameters: method=foods.search&search_expression=toast&format=json // How can I call Api 2.0 with these parameters?
            var responseRequestHere = await responseRequest.Content.ReadAsStringAsync();
            JavaScriptSerializer js = new JavaScriptSerializer();

            SingleObjectJson.Root deserialization = js.Deserialize<SingleObjectJson.Root>(responseRequestHere);
            //SingleObjectJson.Root deserialization = JsonConvert.DeserializeObject<SingleObjectJson.Root>(responseRequestHere);
            var serve = deserialization.food.servings.serving;
            if (serve != null)
            {
                List<SingleObjectJson.Root> listaSingleObjectServing = new List<SingleObjectJson.Root>();

                listaSingleObjectServing.Add(deserialization);

                ViewBag.FoodServingSingle = listaSingleObjectServing;

                return PartialView("DisplayFoodServing");
            }

            Root myDeserializedClass = js.Deserialize<Root>(responseRequestHere);

            //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseRequestHere);

            List<Root> listaFoodServing = new List<Root>();

            listaFoodServing.Add(myDeserializedClass);

            ViewBag.FoodServingData = listaFoodServing;

            return PartialView("DisplayFoodServing");
        }



        [HttpPost]
        public async Task<IActionResult> AddRecipe(CreateRecipeDto model, IFormFile image)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");
            CollectionReference collectionReference = db.Collection("Recipes");

            if (foodlist.Count == 0)
            {
                TempData["Msg"] = "Please Add food to recipe";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                List<string> plans = new List<string>();
                model.plans = plans;

                List<string> food = new List<string>();
                model.food = food;

                //List<FoodApiDtoForRecipe> foods = new List<FoodApiDtoForRecipe>();
                //model.foods = foods;

                foreach (var i in foodlist)
                {
                    model.calcium += i.calcium;
                    model.calories += i.calories;
                    model.carbohydrate += i.carbohydrate;
                    model.cholesterol += i.cholesterol;
                    model.fat += i.fat;
                    model.fiber += i.fiber;
                    model.iron += i.iron;
                    model.monounsaturated_fat += i.monounsaturated_fat;
                    model.polyunsaturated_fat += i.polyunsaturated_fat;
                    model.potassium += i.potassium;
                    model.protein += i.protein;
                    model.saturated_fat += i.saturated_fat;
                    model.sodium += i.sodium;
                    model.sugar += i.sugar;
                    model.vitamin_a += i.vitamin_a;
                    model.vitamin_c += i.vitamin_c;


                    model.foods = i.foods;

                }

                foodlist.Clear();

                List<string> titleSearch = new List<string>();
                titleSearch = Test(model.title);

                //List<string> instructions = new List<string>();
                //foreach (var item in model.instructions)
                //{
                //    if (item != null)
                //    {
                //        List<string> TagIds = item.Split(',').ToList();
                //        instructions = TagIds;
                //    }
                //}
                //model.instructions = instructions;


                if (image == null || image.Length == 0)
                    return Content("file not selected");

                var paths = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

                using (var stream = new FileStream(paths, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                    stream.Flush();
                    stream.Close();
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
                    model.image = link;
                    str.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception was thrown: {0}", ex.Message);
                }

                model.titleSearch = titleSearch;
                var Id = collectionReference.Document().Id;
                model.Id = Id;
                await collectionReference.Document(Id).SetAsync(model, SetOptions.MergeAll);
                return RedirectToAction(nameof(GetRecipe));
            }


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

        public static List<RecipeCalculationDto> foodlist = new List<RecipeCalculationDto>();

        [HttpPost]
        public async Task<IActionResult> AddtoRecipe(RecipeCalculationDto model, RecipeCalcuDto obj, string food, string serving)
        {
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
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
            var responseRequest = await http.GetAsync("https://platform.fatsecret.com/rest/server.api?&food_id=" + food + "&method=food.get.v2&format=json");
            responseRequest.EnsureSuccessStatusCode();

            //Parameters: method=foods.search&search_expression=toast&format=json // How can I call Api 2.0 with these parameters?
            var responseRequestHere = await responseRequest.Content.ReadAsStringAsync();

            JavaScriptSerializer js = new JavaScriptSerializer();

            SingleObjectJson.Root deserialization = js.Deserialize<SingleObjectJson.Root>(responseRequestHere);
            //SingleObjectJson.Root deserialization = JsonConvert.DeserializeObject<SingleObjectJson.Root>(responseRequestHere);
            var serve = deserialization.food.servings.serving;
            if (serve != null)
            {
                List<SingleObjectJson.Root> listaSingleFood = new List<SingleObjectJson.Root>();

                listaSingleFood.Add(deserialization);

                if (listaSingleFood != null)
                {
                    List<FoodApiDtoForRecipe> foodies = new List<FoodApiDtoForRecipe>();

                    List<Dictionary> servingsFood = new List<Dictionary>();

                    foreach (var items in listaSingleFood)
                    {
                        FoodApiDtoForRecipe fd = new FoodApiDtoForRecipe();
                        fd.food_id = items.food.food_id;
                        fd.food_name = items.food.food_name;
                        fd.food_type = items.food.food_type;
                        fd.food_url = items.food.food_url;

                        Dictionary dic = new Dictionary();
                        //foreach (var ser in items.food.servings.serving)
                        //{
                        var ser = items.food.servings.serving;
                        if (ser.serving_id == serving)
                        {
                            dic.calories = ser.calories;
                            dic.carbohydrate = ser.carbohydrate;
                            dic.cholesterol = ser.cholesterol;
                            dic.fat = ser.fat;
                            dic.fiber = ser.fiber;
                            dic.measurement_description = ser.measurement_description;
                            dic.metric_serving_amount = ser.metric_serving_amount;
                            dic.metric_serving_unit = ser.metric_serving_unit;
                            dic.number_of_units = ser.number_of_units;
                            dic.potassium = ser.potassium;
                            dic.protein = ser.protein;
                            dic.saturated_fat = ser.saturated_fat;
                            dic.serving_description = ser.serving_description;
                            dic.sodium = ser.sodium;
                            dic.sugar = ser.sugar;
                            dic.serving_id = ser.serving_id;
                            dic.serving_url = ser.serving_url;
                        }

                        int defaultServing = 0;
                        fd.selectedServing = defaultServing;
                        //}


                        if (items.food.servings.serving != null)
                        {
                            //foreach (var item in items.food.servings.serving)
                            //{
                            var item = items.food.servings.serving;
                            if (item.serving_id == serving)
                            {
                                model.calories = Convert.ToDouble(item.calories);
                                model.carbohydrate = Convert.ToDouble(item.carbohydrate);
                                model.cholesterol = Convert.ToDouble(item.cholesterol);
                                model.fat = Convert.ToDouble(item.fat);
                                model.fiber = Convert.ToDouble(item.fiber);
                                model.potassium = Convert.ToDouble(item.potassium);
                                model.protein = Convert.ToDouble(item.protein);
                                model.saturated_fat = Convert.ToDouble(item.saturated_fat);
                                model.sodium = Convert.ToDouble(item.sodium);
                                model.sugar = Convert.ToDouble(item.sugar);
                                //model.foods = foodies;



                                foreach (var listIn in foodlist)
                                {
                                    foodies = listIn.foods;
                                }

                                servingsFood.Add(dic);
                                fd.servings = servingsFood;
                                foodies.Add(fd);
                                model.foods = foodies;

                                foodlist.Add(model);

                                foreach (var i in foodlist)
                                {
                                    obj.calcium += i.calcium;
                                    obj.calories += i.calories;
                                    obj.carbohydrate += i.carbohydrate;
                                    obj.cholesterol += i.cholesterol;
                                    obj.fat += i.fat;
                                    obj.fiber += i.fiber;
                                    obj.iron += i.iron;
                                    obj.monounsaturated_fat += i.monounsaturated_fat;
                                    obj.polyunsaturated_fat += i.polyunsaturated_fat;
                                    obj.potassium += i.potassium;
                                    obj.protein += i.protein;
                                    obj.saturated_fat += i.saturated_fat;
                                    obj.sodium += i.sodium;
                                    obj.sugar += i.sugar;
                                    obj.vitamin_a += i.vitamin_a;
                                    obj.vitamin_c += i.vitamin_c;


                                }
                            }
                            //}
                        }

                    }
                }
                return PartialView("Views/Recipe/DisplayCalculation.cshtml", obj);
            }


            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseRequestHere);
            //FoodServingJson.Rootobject record = JsonConvert.DeserializeObject<FoodServingJson.Rootobject>(responseRequestHere);

            List<Root> listaFood = new List<Root>();

            listaFood.Add(myDeserializedClass);

            if (listaFood != null)
            {
                List<FoodApiDtoForRecipe> foodies = new List<FoodApiDtoForRecipe>();

                List<Dictionary> servingsFood = new List<Dictionary>();

                foreach (var items in listaFood)
                {
                    FoodApiDtoForRecipe fd = new FoodApiDtoForRecipe();
                    fd.food_id = items.food.food_id;
                    fd.food_name = items.food.food_name;
                    fd.food_type = items.food.food_type;
                    fd.food_url = items.food.food_url;

                    Dictionary dic = new Dictionary();
                    foreach (var ser in items.food.servings.serving)
                    {
                        if (ser.serving_id == serving)
                        {
                            dic.calcium = ser.calcium;
                            dic.calories = ser.calories;
                            dic.carbohydrate = ser.carbohydrate;
                            dic.cholesterol = ser.cholesterol;
                            dic.fat = ser.fat;
                            dic.fiber = ser.fiber;
                            dic.iron = ser.iron;
                            dic.measurement_description = ser.measurement_description;
                            dic.metric_serving_amount = ser.metric_serving_amount;
                            dic.metric_serving_unit = ser.metric_serving_unit;
                            dic.monounsaturated_fat = ser.monounsaturated_fat;
                            dic.number_of_units = ser.number_of_units;
                            dic.polyunsaturated_fat = ser.polyunsaturated_fat;
                            dic.potassium = ser.potassium;
                            dic.protein = ser.protein;
                            dic.saturated_fat = ser.saturated_fat;
                            dic.serving_description = ser.serving_description;
                            dic.sodium = ser.sodium;
                            dic.sugar = ser.sugar;
                            dic.vitamin_a = ser.vitamin_a;
                            dic.vitamin_c = ser.vitamin_c;
                            dic.serving_id = ser.serving_id;
                            dic.serving_url = ser.serving_url;
                        }

                        int defaultServing = 0;
                        fd.selectedServing = defaultServing;

                        //foreach (var ch in listaFood)
                        //{
                        //    for (int i = 0; i < ch.food.servings.serving.Count(); i++)
                        //    {
                        //        foreach (var check in ch.food.servings.serving)
                        //        {
                        //            if (serving == check.serving_id)
                        //            {
                        //                var selectedServing = ch.food.servings.serving.IndexOf(check);

                        //                fd.selectedServing = selectedServing;

                        //            }
                        //        }
                        //    }
                        //}
                    }


                    if (items.food.servings.serving != null)
                    {
                        foreach (var item in items.food.servings.serving)
                        {
                            if (item.serving_id == serving)
                            {
                                model.calcium = Convert.ToDouble(item.calcium);
                                model.calories = Convert.ToDouble(item.calories);
                                model.carbohydrate = Convert.ToDouble(item.carbohydrate);
                                model.cholesterol = Convert.ToDouble(item.cholesterol);
                                model.fat = Convert.ToDouble(item.fat);
                                model.fiber = Convert.ToDouble(item.fiber);
                                model.iron = Convert.ToDouble(item.iron);
                                model.monounsaturated_fat = Convert.ToDouble(item.monounsaturated_fat);
                                model.polyunsaturated_fat = Convert.ToDouble(item.polyunsaturated_fat);
                                model.potassium = Convert.ToDouble(item.potassium);
                                model.protein = Convert.ToDouble(item.protein);
                                model.saturated_fat = Convert.ToDouble(item.saturated_fat);
                                model.sodium = Convert.ToDouble(item.sodium);
                                model.sugar = Convert.ToDouble(item.sugar);
                                model.vitamin_a = Convert.ToDouble(item.vitamin_a);
                                model.vitamin_c = Convert.ToDouble(item.vitamin_c);
                                //model.foods = foodies;



                                foreach (var listIn in foodlist)
                                {
                                    foodies = listIn.foods;
                                }

                                servingsFood.Add(dic);
                                fd.servings = servingsFood;
                                foodies.Add(fd);
                                model.foods = foodies;

                                foodlist.Add(model);

                                foreach (var i in foodlist)
                                {
                                    obj.calcium += i.calcium;
                                    obj.calories += i.calories;
                                    obj.carbohydrate += i.carbohydrate;
                                    obj.cholesterol += i.cholesterol;
                                    obj.fat += i.fat;
                                    obj.fiber += i.fiber;
                                    obj.iron += i.iron;
                                    obj.monounsaturated_fat += i.monounsaturated_fat;
                                    obj.polyunsaturated_fat += i.polyunsaturated_fat;
                                    obj.potassium += i.potassium;
                                    obj.protein += i.protein;
                                    obj.saturated_fat += i.saturated_fat;
                                    obj.sodium += i.sodium;
                                    obj.sugar += i.sugar;
                                    obj.vitamin_a += i.vitamin_a;
                                    obj.vitamin_c += i.vitamin_c;


                                }
                            }
                        }
                    }

                }
            }
            return PartialView("Views/Recipe/DisplayCalculation.cshtml", obj);
        }


        public async Task<IActionResult> GetRecipe()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

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
                }
            }
            return View(listaRecipe);
        }

        [HttpPost]
        public async Task<IActionResult> AddMultipleFood(string[] addFood, string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference washingtonRef = db.Collection("Recipes").Document(Id);

            await washingtonRef.UpdateAsync("addFood", FieldValue.ArrayUnion(addFood));

            return View("EditFood");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentreference = db.Collection("Recipes").Document(Id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<CreateRecipe> listaRecipe = new List<CreateRecipe>();

            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                CreateRecipe std = JsonConvert.DeserializeObject<CreateRecipe>(json);
                std.recipeId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                //List<string> str = new List<string>();
                //var sd = string.Join(',', std.instructions);
                //str.Add(sd);
                //std.instructions = str;

                listaRecipe.Add(std);

                ViewBag.Add = listaRecipe;


                Query docref = db.Collection("Foods");
                QuerySnapshot snap = await docref.GetSnapshotAsync();
                List<CreateFoodDto> listaFood = new List<CreateFoodDto>();
                foreach (DocumentSnapshot documentSnapshot1 in snap.Documents)
                {
                    if (documentSnapshot1.Exists)
                    {
                        Dictionary<string, object> cat1 = documentSnapshot1.ToDictionary();
                        string json1 = JsonConvert.SerializeObject(cat1);
                        CreateFoodDto std1 = JsonConvert.DeserializeObject<CreateFoodDto>(json1);
                        std1.FoodId = documentSnapshot1.Id;
                        //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();


                        listaFood.Add(std1);
                        ViewBag.Food = listaFood;

                    }
                }

                Query documentref = db.Collection("Filters");
                QuerySnapshot snaps = await documentref.GetSnapshotAsync();
                List<Filter> listaFilter = new List<Filter>();
                foreach (DocumentSnapshot docSnapshot in snaps.Documents)
                {
                    if (docSnapshot.Exists)
                    {
                        Dictionary<string, object> filter = docSnapshot.ToDictionary();
                        string jsonData = JsonConvert.SerializeObject(filter);
                        Filter std2 = JsonConvert.DeserializeObject<Filter>(jsonData);
                        std2.filterId = docSnapshot.Id;
                        //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();


                        listaFilter.Add(std2);
                        ViewBag.Filter = listaFilter;
                    }
                }
            }

            return View(listaRecipe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RecipeDto model, IFormFile image, string image1)
        {
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("Recipes").Document(model.recipeId);
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();
            List<RecipeCalculationDto> listaRecipe = new List<RecipeCalculationDto>();

            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                RecipeCalculationDto std = JsonConvert.DeserializeObject<RecipeCalculationDto>(json);
                std.recipeId = documentSnapshot.Id;

                listaRecipe.Add(std);

                List<string> food = new List<string>();
                model.food = food;

                //List<FoodDtoForRecipe> foods = new List<FoodDtoForRecipe>();
                //model.foods = foods;

                foreach (var it in listaRecipe)
                {
                    foreach (var i in foodlist)
                    {
                        it.calcium += i.calcium;
                        it.calories += i.calories;
                        it.carbohydrate += i.carbohydrate;
                        it.cholesterol += i.cholesterol;
                        it.fat += i.fat;
                        it.fiber += i.fiber;
                        it.iron += i.iron;
                        it.monounsaturated_fat += i.monounsaturated_fat;
                        it.polyunsaturated_fat += i.polyunsaturated_fat;
                        it.potassium += i.potassium;
                        it.protein += i.protein;
                        it.saturated_fat += i.saturated_fat;
                        it.sodium += i.sodium;
                        it.sugar += i.sugar;
                        it.vitamin_a += i.vitamin_a;
                        it.vitamin_c += i.vitamin_c;


                        foreach (var v in i.foods)
                        {
                            it.foods.Add(v);
                        }

                    }
                    model.calcium += it.calcium;
                    model.calories += it.calories;
                    model.carbohydrate += it.carbohydrate;
                    model.cholesterol += it.cholesterol;
                    model.fat += it.fat;
                    model.fiber += it.fiber;
                    model.iron += it.iron;
                    model.monounsaturated_fat += it.monounsaturated_fat;
                    model.polyunsaturated_fat += it.polyunsaturated_fat;
                    model.potassium += it.potassium;
                    model.protein += it.protein;
                    model.saturated_fat += it.saturated_fat;
                    model.sodium += it.sodium;
                    model.sugar += it.sugar;
                    model.vitamin_a += it.vitamin_a;
                    model.vitamin_c += it.vitamin_c;

                    model.foods = it.foods;

                }

                foodlist.Clear();

            }


            //List<string> instructions = new List<string>();
            //foreach (var item in model.instructions)
            //{
            //    if (item != null)
            //    {
            //        List<string> TagIds = item.Split(',').ToList();
            //        instructions = TagIds;
            //    }
            //}
            //model.instructions = instructions;


            if (image == null || image.Length == 0)
            {
                model.image = image1;
            }
            else
            {
                var paths = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

                using (var stream = new FileStream(paths, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                    stream.Flush();
                    stream.Close();
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
                    model.image = link;
                    str.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception was thrown: {0}", ex.Message);
                }
            }

            await documentReference.SetAsync(model, SetOptions.MergeAll);
            return RedirectToAction(nameof(GetRecipe));
        }

        public async Task<IActionResult> EditFood(string id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentreference = db.Collection("Recipes").Document(id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<CreateRecipeDto> listaFoods = new List<CreateRecipeDto>();
            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                CreateRecipeDto std = JsonConvert.DeserializeObject<CreateRecipeDto>(json);
                std.recipeId = documentSnapshot.Id;

                listaFoods.Add(std);
            }

            return View(listaFoods);

        }


        public async Task<IActionResult> DeleteFood(RecipeArrayDto model, string Id, string serving_id, string food_id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference washingtonRef = db.Collection("Recipes").Document(Id);

            DocumentSnapshot documentSnapshot = await washingtonRef.GetSnapshotAsync();
            List<RecipeArrayDto> listaddFood = new List<RecipeArrayDto>();
            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                RecipeArrayDto std = JsonConvert.DeserializeObject<RecipeArrayDto>(json);


                //std.FoodId = documentSnapshot.Id;
                listaddFood.Add(std);

                ViewBag.addFood = listaddFood;

                List<FoodApiDtoForRecipe> addFoodsArray = new List<FoodApiDtoForRecipe>();

                foreach (var item in ViewBag.addFood)
                {
                    if (item.foods.Count == 1)
                    {
                        TempData["Msg"] = "Last Food Item Cannot Be Deleted!";
                        return RedirectToAction("EditFood", new { Id });
                    }
                }

                var flg = 0;
                foreach (var v in listaddFood)
                {
                    foreach (var va in v.foods)
                    {
                        foreach (var ser in va.servings)
                        {
                            if (va.food_id == food_id && ser.serving_id == serving_id)
                            {
                                if (flg == 0)
                                {

                                }
                                else
                                {
                                    addFoodsArray.Add(va);
                                    model.foods = addFoodsArray;
                                }
                                flg = 1;
                            }
                            else
                            {
                                addFoodsArray.Add(va);
                                model.foods = addFoodsArray;
                            }
                        }


                    }
                }

                foreach (var m in listaddFood)
                {
                    foreach (var n in m.foods)
                    {
                        foreach (var ser in n.servings)
                        {
                            if (n.food_id == food_id && ser.serving_id == serving_id)
                            {
                                var calcium = Convert.ToDouble(ser.calcium);
                                var calories = Convert.ToDouble(ser.calories);
                                var carbohydrate = Convert.ToDouble(ser.carbohydrate);
                                var cholesterol = Convert.ToDouble(ser.cholesterol);
                                var fat = Convert.ToDouble(ser.fat);
                                var fiber = Convert.ToDouble(ser.fiber);
                                var iron = Convert.ToDouble(ser.iron);
                                var monounsaturated_fat = Convert.ToDouble(ser.monounsaturated_fat);
                                var polyunsaturated_fat = Convert.ToDouble(ser.polyunsaturated_fat);
                                var potassium = Convert.ToDouble(ser.potassium);
                                var protein = Convert.ToDouble(ser.protein);
                                var saturated_fat = Convert.ToDouble(ser.saturated_fat);
                                var sodium = Convert.ToDouble(ser.sodium);
                                var sugar = Convert.ToDouble(ser.sugar);
                                var vitamin_a = Convert.ToDouble(ser.vitamin_a);
                                var vitamin_c = Convert.ToDouble(ser.vitamin_c);


                                model.calcium = m.calcium - calcium;
                                model.calories = m.calories - calories;
                                model.carbohydrate = m.carbohydrate - carbohydrate;
                                model.cholesterol = m.cholesterol - cholesterol;
                                model.fat = m.fat - fat;
                                model.fiber = m.fiber - fiber;
                                model.iron = m.iron - iron;
                                model.monounsaturated_fat = m.monounsaturated_fat - monounsaturated_fat;
                                model.polyunsaturated_fat = m.polyunsaturated_fat - polyunsaturated_fat;
                                model.potassium = m.potassium - potassium;
                                model.protein = m.protein - protein;
                                model.saturated_fat = m.saturated_fat - saturated_fat;
                                model.sodium = m.sodium - sodium;
                                model.sugar = m.sugar - sugar;
                                model.vitamin_a = m.vitamin_a - vitamin_a;
                                model.vitamin_c = m.vitamin_c - vitamin_c;

                            }
                        }
                       
                    }
                }
            }

            await washingtonRef.SetAsync(model, SetOptions.MergeAll);
            TempData["Msg"] = "Food Deleted!";
            return RedirectToAction("EditFood", new { Id });
        }


        public async Task<IActionResult> Delete(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("Recipes").Document(Id);
            await documentReference.DeleteAsync();
            TempData["Msg"] = "Recipe Deleted!";
            return RedirectToAction(nameof(GetRecipe));
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
        public async Task<IActionResult> AddPlantoRecipe(RecipePlan model, string recipeId)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentReference = db.Collection("Recipes").Document(recipeId);
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();
            List<RecipePlan> listaRecipes = new List<RecipePlan>();

            List<string> plans = new List<string>();
            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                RecipePlan std = JsonConvert.DeserializeObject<RecipePlan>(json);
                std.recipeId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                listaRecipes.Add(std);

                foreach (var lists in listaRecipes)
                {
                    if (lists.plans != null)
                    {
                        foreach (var list in lists.plans)
                        {
                            foreach (var r in model.plans)
                            {
                                if (list == r)
                                {
                                    TempData["Msg"] = "Plan Already Exist!";
                                    return RedirectToAction("PlanRecipes");
                                }
                            }

                        }
                    }

                }

                foreach (var items in listaRecipes)
                {
                    if (items.plans != null)
                    {
                        foreach (var item in items.plans)
                        {
                            model.plans.Add(item);
                        }
                    }

                }

            }
            await documentReference.SetAsync(model, SetOptions.MergeAll);
            return RedirectToAction(nameof(PlanRecipes));

        }

        [HttpGet]
        public async Task<IActionResult> EditPlans(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentreference = db.Collection("Recipes").Document(Id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<RecipePlan> listaRecipes = new List<RecipePlan>();

            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                RecipePlan std = JsonConvert.DeserializeObject<RecipePlan>(json);
                std.recipeId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                listaRecipes.Add(std);
            }

            Query docref = db.Collection("Plans");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<Plans> listaPlans = new List<Plans>();
            foreach (DocumentSnapshot documentSnap in snap.Documents)
            {
                if (documentSnap.Exists)
                {
                    Dictionary<string, object> cat = documentSnap.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    Plans std = JsonConvert.DeserializeObject<Plans>(json);
                    std.plansId = documentSnap.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaPlans.Add(std);
                    ViewBag.Plans = listaPlans;

                }
            }

            return View(listaRecipes);
        }

        [HttpPost]

        public async Task<IActionResult> EditPlans(RecipePlan model, string recipeId)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("Recipes").Document(recipeId);

            await documentReference.SetAsync(model, SetOptions.MergeAll);
            return RedirectToAction(nameof(GetRecipe));

        }

    }
}