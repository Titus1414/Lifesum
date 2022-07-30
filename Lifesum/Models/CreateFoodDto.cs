using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{

    [FirestoreData]
    public class CreateFoodDto
    {
        [FirestoreDocumentId]
        public string FoodId { get; set; }
        [FirestoreProperty]
        public string title { get; set; }
        [FirestoreProperty]
        public string Image { get; set; }
        [FirestoreProperty]
        public string brand { get; set; }
        [FirestoreProperty]
        public string barcode { get; set; }
        [FirestoreProperty]
        public string calories { get; set; }
    }
}
