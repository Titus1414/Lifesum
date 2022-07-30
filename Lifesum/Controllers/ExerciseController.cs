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
    public class ExerciseController : Controller
    {
        FirestoreDb db;
        public IActionResult Index()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            return View();
        }

        public async Task<IActionResult> AddExercise(Exercise model)
        {
            db = FirestoreDb.Create("lifesum-99433");
            CollectionReference collectionReference = db.Collection("Exercise");

            Query docref = db.Collection("Exercise");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<Exercise> listaExercise = new List<Exercise>();
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    Exercise std = JsonConvert.DeserializeObject<Exercise>(json);
                    std.exerciseId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaExercise.Add(std);

                    foreach (var item in listaExercise)
                    {
                        if (model.title == item.title)
                        {
                            TempData["Msg"] = "Exercise exist!";
                            return RedirectToAction(nameof(GetExercise));
                        }
                    }
                }
            }

            await collectionReference.AddAsync(model);
            return RedirectToAction(nameof(GetExercise));
        }

        public async Task<IActionResult> GetExercise()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            Query docref = db.Collection("Exercise");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<Exercise> listaExercise = new List<Exercise>();
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    Exercise std = JsonConvert.DeserializeObject<Exercise>(json);
                    std.exerciseId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaExercise.Add(std);
                }
            }
            return View(listaExercise);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentreference = db.Collection("Exercise").Document(Id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            Exercise listaExercise = new Exercise();

            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                Exercise std = JsonConvert.DeserializeObject<Exercise>(json);
                std.exerciseId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();

                listaExercise = std;

            }
            return View(listaExercise);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Exercise obj)
        {
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("Exercise").Document(obj.exerciseId);

            //Query docref = db.Collection("Exercise");
            //QuerySnapshot snap = await docref.GetSnapshotAsync();
            //List<Exercise> listaExercise = new List<Exercise>();
            //foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            //{
            //    if (documentSnapshot.Exists)
            //    {
            //        Dictionary<string, object> cat = documentSnapshot.ToDictionary();
            //        string json = JsonConvert.SerializeObject(cat);
            //        Exercise std = JsonConvert.DeserializeObject<Exercise>(json);
            //        std.exerciseId = documentSnapshot.Id;
            //        //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

            //        listaExercise.Add(std);

            //        foreach (var item in listaExercise)
            //        {
            //            if (obj.title == item.title)
            //            {
            //                //TempData["Msg"] = "Edit Successfully!";
            //                return RedirectToAction(nameof(GetExercise));
            //            }

            //        }
            //    }
            //}

            await documentReference.SetAsync(obj, SetOptions.MergeAll);
            return RedirectToAction(nameof(GetExercise));
        }

        public async Task<IActionResult> Delete(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("Exercise").Document(Id);
            await documentReference.DeleteAsync();
            TempData["Msg"] = "Exercise Deleted!";
            return RedirectToAction(nameof(GetExercise));
        }


    }
}
