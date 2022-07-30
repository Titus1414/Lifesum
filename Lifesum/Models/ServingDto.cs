using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class ServingDto
    {
        [FirestoreDocumentId]
        public string FoodId { get; set; }
        [FirestoreProperty]
        public string servingName { get; set; }

        [FirestoreProperty]
        public string title { get; set; }

        [FirestoreProperty]
        public string unit { get; set; }
        [FirestoreProperty]
        public int value { get; set; }
    }
}
