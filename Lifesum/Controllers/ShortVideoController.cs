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
using Firebase.Storage;
using System.Threading;
using Firebase.Auth;

namespace Lifesum.Controllers
{
    public class ShortVideoController : Controller
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

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576000)]
        public async Task<IActionResult> AddShortVideo(ShortVideo model, IFormFile video, IFormFile thumbnail, double infos)
        {
            db = FirestoreDb.Create("lifesum-99433");
            CollectionReference collectionReference = db.Collection("ShortVideos");

            if (thumbnail == null || thumbnail.Length == 0)
                return Content("file not selected");

            var paths = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/images", thumbnail.FileName);

            using (var stream = new FileStream(paths, FileMode.Create))
            {
                await thumbnail.CopyToAsync(stream);
                stream.Flush();
                stream.Close();
            }

            string filename = thumbnail.FileName;

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
                model.thumbnail = link;
                str.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex.Message);
            }


            if (video == null || video.Length == 0)
                return Content("file not supported");

            if(infos > 30)
            {
                TempData["Msg"] = "Video duration should be less than or equal to 30 seconds !";
                return RedirectToAction(nameof(Index));
            }

            var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/Videos", video.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await video.CopyToAsync(stream);
                stream.Flush();
                stream.Close();
            }

            string filename1 = video.FileName;

            FileStream str1;
            str1 = new FileStream(path, FileMode.Open);

            var auth1 = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a1 = await auth1.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var cancellation1 = new CancellationTokenSource();

            var task1 = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a1.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("videos")
                .Child("ShortVideos")
                .Child(filename1)
                .PutAsync(str1, cancellation1.Token);

            try
            {
                string link1 = await task1;
                model.video = link1;
                str1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex.Message);
            }


            await collectionReference.AddAsync(model);
            return RedirectToAction(nameof(GetShortVideos));
        }

        [HttpGet]

        public async Task<IActionResult> GetShortVideos()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("lifesum-99433");

            Query docref = db.Collection("ShortVideos");
            QuerySnapshot snap = await docref.GetSnapshotAsync();
            List<ShortVideo> listaVideos = new List<ShortVideo>();
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(cat);
                    ShortVideo std = JsonConvert.DeserializeObject<ShortVideo>(json);
                    std.videoId = documentSnapshot.Id;
                    //MydocLst list = documentSnapshot.ConvertTo<MydocLst>();

                    listaVideos.Add(std);
                }
            }
            return View(listaVideos);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");

            DocumentReference documentreference = db.Collection("ShortVideos").Document(Id);
            DocumentSnapshot documentSnapshot = await documentreference.GetSnapshotAsync();
            List<ShortVideo> listaShortVideos = new List<ShortVideo>();

            if (documentSnapshot.Exists)
            {
                Dictionary<string, object> cat = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(cat);
                ShortVideo std = JsonConvert.DeserializeObject<ShortVideo>(json);
                std.videoId = documentSnapshot.Id;
                //Category stud = documentSnapshot.ConvertTo<Category>();


                listaShortVideos.Add(std);

                ViewBag.ShortVideos = listaShortVideos;

            }

            return View(listaShortVideos);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576000)]
        public async Task<IActionResult> Edit(ShortVideo model, IFormFile video, IFormFile thumbnail, string thumbnail1, string video1, double infos)
        {
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("ShortVideos").Document(model.videoId);

            if (thumbnail == null || thumbnail.Length == 0)
            {
                model.thumbnail = thumbnail1;
            }
            else
            {
                var paths = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/images", thumbnail.FileName);

                using (var stream = new FileStream(paths, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(stream);
                    stream.Flush();
                    stream.Close();
                }

                string filename = thumbnail.FileName;

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
                    model.thumbnail = link;
                    str.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception was thrown: {0}", ex.Message);
                }
            }
            
            if (video == null || video.Length == 0)
            {
                model.video = video1;
            }
            else
            {
                if(infos > 30)
                {
                    TempData["Msg"] = "Video duration should be less than or equal to 30 seconds !";
                    return RedirectToAction("Edit", new { id = model.videoId });
                }

                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/Videos", video.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await video.CopyToAsync(stream);
                    stream.Flush();
                    stream.Close();
                }

                string filename1 = video.FileName;

                FileStream str1;
                str1 = new FileStream(path, FileMode.Open);

                var auth1 = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a1 = await auth1.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                var cancellation1 = new CancellationTokenSource();

                var task1 = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a1.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child("videos")
                    .Child("ShortVideos")
                    .Child(filename1)
                    .PutAsync(str1, cancellation1.Token);

                try
                {
                    string link1 = await task1;
                    model.video = link1;
                    str1.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception was thrown: {0}", ex.Message);
                }
            }
            
            await documentReference.SetAsync(model, SetOptions.MergeAll);
            return RedirectToAction(nameof(GetShortVideos));
        }

       
        public async Task<IActionResult> Delete(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"lifesum.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("lifesum-99433");
            DocumentReference documentReference = db.Collection("ShortVideos").Document(Id);
            await documentReference.DeleteAsync();
            TempData["Msg"] = "Short Video Deleted!";
            return RedirectToAction(nameof(GetShortVideos));
        }

    }
}
