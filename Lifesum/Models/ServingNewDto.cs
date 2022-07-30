using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class ServingNewDto
    {
        [FirestoreDocumentId]
        public string FoodId { get; set; }
        [FirestoreProperty]
        public List<Dictionary> serving { get; set; }

    }
}
