using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class Filter
    {
        [FirestoreDocumentId]
        public string filterId { get; set; }

        [FirestoreProperty]
        public List<string> filters { get; set; }
    }
}
