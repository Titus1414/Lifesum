using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class RecipeCalcuDto
    {
        [FirestoreDocumentId]
        public string recipeId { get; set; }

        [FirestoreProperty]
        public List<string> food { get; set; }

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
        public double measurement_description { get; set; }

        [FirestoreProperty]
        public double metric_serving_amount { get; set; }

        [FirestoreProperty]
        public double metric_serving_unit { get; set; }

        [FirestoreProperty]
        public double monounsaturated_fat { get; set; }

        [FirestoreProperty]
        public double number_of_units { get; set; }

        [FirestoreProperty]
        public double polyunsaturated_fat { get; set; }

        [FirestoreProperty]
        public double potassium { get; set; }

        [FirestoreProperty]
        public double protein { get; set; }

        [FirestoreProperty]
        public double saturated_fat { get; set; }

        [FirestoreProperty]
        public double serving_description { get; set; }

        [FirestoreProperty]
        public double serving_id { get; set; }

        [FirestoreProperty]
        public double serving_url { get; set; }

        [FirestoreProperty]
        public double sodium { get; set; }

        [FirestoreProperty]
        public double sugar { get; set; }

        [FirestoreProperty]
        public double vitamin_a { get; set; }

        [FirestoreProperty]
        public double vitamin_c { get; set; }

        [FirestoreProperty]
        public double serving { get; set; }


    }
}
