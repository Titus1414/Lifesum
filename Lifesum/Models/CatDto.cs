using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class CatDto
    {
        [FirestoreDocumentId]
        public string CatId { get; set; }

        [FirestoreProperty]
        public string title { get; set; }
        [FirestoreProperty]
        public List<string> servings { get; set; }
        [FirestoreProperty]
        public List<string> subcategories { get; set; }
    }
}
