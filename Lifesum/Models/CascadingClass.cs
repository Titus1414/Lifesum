using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Lifesum.Models
{
    [FirestoreData]
    public class CascadingClass
    {
        [FirestoreProperty]
        public string CategoryId { get; set; }

        [FirestoreProperty]
        public string CatId { get; set; }

        [FirestoreProperty]
        public string SerId { get; set; }

        [FirestoreProperty]
        public string FoodId { get; set; }
    }
}
