using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class FoodServingDto
    {
        [FirestoreDocumentId]
        public string MyProperty { get; set; }
    }
}
