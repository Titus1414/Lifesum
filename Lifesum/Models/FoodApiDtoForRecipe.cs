using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]

    public class FoodApiDtoForRecipe
    {
        [FirestoreDocumentId]
        public string FoodId { get; set; }

        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string food_id { get; set; }
        
        [FirestoreProperty]
        public string food_name { get; set; }

        [FirestoreProperty]
        public string food_type { get; set; }
        
        [FirestoreProperty]
        public string food_url { get; set; }

        [FirestoreProperty]
        public int selectedServing { get; set; }

        [FirestoreProperty]
        public List<Dictionary> servings { get; set; }

    }
}
