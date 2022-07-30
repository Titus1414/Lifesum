using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Lifesum.Models
{
    [FirestoreData]
    public class CreateFood
    {
        [FirestoreDocumentId]
        public string FoodId { get; set; }

        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Image { get; set; }

        [FirestoreProperty]
        public string title { get; set; }

        [FirestoreProperty]
        public string brand { get; set; }

        [FirestoreProperty]
        public string category { get; set; }

        [FirestoreProperty]
        public string subcategory { get; set; }

        [FirestoreProperty]
        public string barcode { get; set; }

        [FirestoreProperty]
        public double calories { get; set; }

        [FirestoreProperty]
        public double protein { get; set; }

        [FirestoreProperty]
        public double carbs { get; set; }

        [FirestoreProperty]
        public double fat { get; set; }

        [FirestoreProperty]
        public double saturatedfat { get; set; }

        [FirestoreProperty]
        public double unsaturatedfat { get; set; }

        [FirestoreProperty]
        public double fiber { get; set; }

        [FirestoreProperty]
        public double cholesterol { get; set; }

        [FirestoreProperty]
        public double potassium { get; set; }

        [FirestoreProperty]
        public double sodium { get; set; }

        [FirestoreProperty]
        public double sugars { get; set; }

        [FirestoreProperty]
        public List<Dictionary> serving { get; set; }

        [FirestoreProperty]
        public List<string> titlesSearch { get; set; }

        [FirestoreProperty]
        public List<string> brandSearch { get; set; }

    }
}
