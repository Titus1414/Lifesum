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


namespace Lifesum.Controllers
{
    public class FilterController : Controller
    {
        FirestoreDb db;
        public IActionResult Index()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            return View();
        }

        public async Task<IActionResult> AddFilters(Filter model)
        {
            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentReference = db.Collection("Filter").Document("Filter");

            List<string> filters = new List<string>();
            foreach (var item in model.filters)
            {
                if (item != null)
                {
                    List<string> TagIds = item.Split(',').ToList();
                    filters = TagIds;
                }
            }

            Query docref = db.Collection("Filter");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<Filter> listaFilters = new List<Filter>();
            foreach (DocumentSnapshot documentSnap in snap.Documents)
            {
                if (documentSnap.Exists)
                {
                    Dictionary<string, object> cat = documentSnap.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    Filter std = JsonConvert.DeserializeObject<Filter>(json);
                    std.filterId = documentSnap.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaFilters.Add(std);

                    foreach (var item in filters)
                    {
                        foreach (var items in listaFilters)
                        {
                            if(items.filters!=null)
                            {
                                foreach (var i in items.filters)
                                {
                                    if (item == i)
                                    {
                                        TempData["Msg"] = "Filter exist!";
                                        return RedirectToAction(nameof(GetFilters));
                                    }

                                }
                            }
                            
                        }
                    }
                }
            }

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();
            List<Filter> listaFilter = new List<Filter>();
            List<string> filterarray = new List<string>();
            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                Filter std = JsonConvert.DeserializeObject<Filter>(json);


                //std.FoodId = documentSnapshot.Id;
                listaFilter.Add(std);
                ViewBag.Filter = listaFilter;

                foreach (var item in ViewBag.Filter)
                {
                    if(item.filters!=null)
                    {
                        foreach (var i in item.filters)
                        {
                            filterarray = filters;
                            filterarray.Add(i);
                        }
                    }

                }

            }
            model.filters = filters;
            await documentReference.SetAsync(model);
            return RedirectToAction("GetFilters");
        }


        public async Task<IActionResult> GetFilters()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            Query docref = db.Collection("Filter");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<Filter> listaFilter = new List<Filter>();
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    Filter std = JsonConvert.DeserializeObject<Filter>(json);
                    std.filterId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaFilter.Add(std);
                }
            }
            return View(listaFilter);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Filter model, string Id, string Indx)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference washingtonRef = db.Collection("Filter").Document(Id);

            DocumentSnapshot documentSnapshot = await washingtonRef.GetSnapshotAsync();
            List<Filter> listaFilter = new List<Filter>();
            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                Filter std = JsonConvert.DeserializeObject<Filter>(json);


                //std.FoodId = documentSnapshot.Id;
                listaFilter.Add(std);

                ViewBag.arrFilter = listaFilter;

                List<string> filtersArray = new List<string>();

                foreach (var item in ViewBag.arrFilter)
                {
                    foreach (var i in item.filters)
                    {
                        if (i != Indx)
                        {
                            filtersArray.Add(i);
                            model.filters = filtersArray;
                        }
                    }
                }

            }

            await washingtonRef.SetAsync(model, SetOptions.MergeAll);
            TempData["Msg"] = "Filter Deleted!";
            return RedirectToAction("GetFilters");
        }


    }
}
