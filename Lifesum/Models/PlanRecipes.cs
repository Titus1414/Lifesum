using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;


namespace Lifesum.Models
{
    [FirestoreData]
    public class PlanRecipes
    {
        [FirestoreDocumentId]
        public string plansId { get; set; }

        [FirestoreProperty]
        public List<string> recipes { get; set; }
    }
}
