using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class ShortVideo
    {
        [FirestoreDocumentId]
        public string videoId { get; set; }
        
        [FirestoreProperty]
        public string title { get; set; }

        [FirestoreProperty]
        public string thumbnail { get; set; }

        [FirestoreProperty]
        public string video { get; set; }

        [FirestoreProperty]
        public string bodyPart { get; set; }
    }
}
