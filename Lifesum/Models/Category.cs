using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class Category
    {
        [FirestoreDocumentId]
        public string CategoryId { get; set; }

        [FirestoreProperty]
        public string title { get; set; }
        [FirestoreProperty]
        public List<string> servings { get; set; }
        [FirestoreProperty]
        public List<string> subcategories { get; set; }
    }
}
