using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    public class FoodDtoForMeal
    {
        [FirestoreDocumentId]
        public string FoodId { get; set; }

        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string title { get; set; }

        [FirestoreProperty]
        public double calcium { get; set; }

        [FirestoreProperty]
        public double calories { get; set; }

        [FirestoreProperty]
        public double carbohydrate { get; set; }

        [FirestoreProperty]
        public double cholesterol { get; set; }

        [FirestoreProperty]
        public double fat { get; set; }

        [FirestoreProperty]
        public double fiber { get; set; }

        [FirestoreProperty]
        public double iron { get; set; }

        [FirestoreProperty]
        public double monounsaturated_fat { get; set; }

        [FirestoreProperty]
        public double polyunsaturated_fat { get; set; }

        [FirestoreProperty]
        public double potassium { get; set; }

        [FirestoreProperty]
        public double protein { get; set; }

        [FirestoreProperty]
        public double saturated_fat { get; set; }

        [FirestoreProperty]
        public double sodium { get; set; }

        [FirestoreProperty]
        public double sugar { get; set; }

        [FirestoreProperty]
        public double vitamin_a { get; set; }

        [FirestoreProperty]
        public double vitamin_c { get; set; }

        [FirestoreProperty]
        public int selectedServing { get; set; }

        [FirestoreProperty]
        public List<Dictionary> serving { get; set; }

        [FirestoreProperty]
        public List<string> titlesSearch { get; set; }

    }
}
