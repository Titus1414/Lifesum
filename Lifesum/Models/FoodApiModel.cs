using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Lifesum.Models
{
    [FirestoreData]
    public class FoodApiModel
    {
        [FirestoreDocumentId]
        public string Food_Id { get; set; }

        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string food_name { get; set; }

        [FirestoreProperty]
        public string food_type { get; set; }

        [FirestoreProperty]
        public string food_url { get; set; }

        [FirestoreProperty]
        public List<FoodDictionary> servings { get; set; }

        [FirestoreProperty]
        public string selectedServing { get; set; }

        [FirestoreProperty]
        public bool ismine { get; set; }

    }
}
