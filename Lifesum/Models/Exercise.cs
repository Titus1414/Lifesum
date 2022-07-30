using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class Exercise
    {
        [FirestoreDocumentId]
        public string exerciseId { get; set; }

        [FirestoreProperty]
        public string title { get; set; }

        [FirestoreProperty]
        public double calories { get; set; }
    }
}
