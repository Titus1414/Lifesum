using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class Meal
    {
        [FirestoreDocumentId]
        public string mealId { get; set; }

        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public string image { get; set; }

        [FirestoreProperty]
        public List<string> food { get; set; }

        [FirestoreProperty]
        public List<FoodApiDtoForRecipe> foods { get; set; }

        //[FirestoreProperty]
        //public double calories { get; set; }

        //[FirestoreProperty]
        //public double carbs { get; set; }

        //[FirestoreProperty]
        //public double cholesterol { get; set; }

        //[FirestoreProperty]
        //public double fat { get; set; }

        //[FirestoreProperty]
        //public double fiber { get; set; }

        //[FirestoreProperty]
        //public double potassium { get; set; }

        //[FirestoreProperty]
        //public double protein { get; set; }

        //[FirestoreProperty]
        //public double saturatedfat { get; set; }

        //[FirestoreProperty]
        //public double sodium { get; set; }

        //[FirestoreProperty]
        //public double unsaturatedfat { get; set; }

        //[FirestoreProperty]
        //public double sugar { get; set; }
    }
}
