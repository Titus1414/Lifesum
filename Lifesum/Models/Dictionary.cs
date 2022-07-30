using Google.Cloud.Firestore;
using System.Collections.Generic;

namespace Lifesum.Models
{
    [FirestoreData]
    public class Dictionary
    {
        //[FirestoreProperty]
        //public string servingName { get; set; }

        //[FirestoreProperty]
        //public string unit { get; set; }

        //[FirestoreProperty]
        //public double value { get; set; }

        [FirestoreProperty]
        public string calcium { get; set; }

        [FirestoreProperty]
        public string calories { get; set; }

        [FirestoreProperty]
        public string carbohydrate { get; set; }

        [FirestoreProperty]
        public string cholesterol { get; set; }

        [FirestoreProperty]
        public string fat { get; set; }

        [FirestoreProperty]
        public string fiber { get; set; }

        [FirestoreProperty]
        public string iron { get; set; }

        [FirestoreProperty]
        public string monounsaturated_fat { get; set; }

        [FirestoreProperty]
        public string polyunsaturated_fat { get; set; }

        [FirestoreProperty]
        public string potassium { get; set; }

        [FirestoreProperty]
        public string protein { get; set; }

        [FirestoreProperty]
        public string saturated_fat { get; set; }

        [FirestoreProperty]
        public List<string> servings { get; set; }

        [FirestoreProperty]
        public string sodium { get; set; }

        [FirestoreProperty]
        public string sugar { get; set; }

        [FirestoreProperty]
        public string vitamin_a { get; set; }

        [FirestoreProperty]
        public string vitamin_c { get; set; }

        [FirestoreProperty]
        public string measurement_description { get; set; }

        [FirestoreProperty]
        public string metric_serving_amount { get; set; }

        [FirestoreProperty]
        public string metric_serving_unit { get; set; }

        [FirestoreProperty]
        public string number_of_units { get; set; }

        [FirestoreProperty]
        public string serving_description { get; set; }

        [FirestoreProperty]
        public string serving_id { get; set; }

        [FirestoreProperty]
        public string serving_url { get; set; }

    }
}