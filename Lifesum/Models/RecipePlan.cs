using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;


namespace Lifesum.Models
{
    [FirestoreData]
    public class RecipePlan
    {
        [FirestoreDocumentId]
        public string recipeId { get; set; }

        [FirestoreProperty]
        public List<string> plans { get; set; }
    }
}
