using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class PlansFocus
    {
        [FirestoreDocumentId]
        public string focusPlansId { get; set; }

        [FirestoreProperty]
        public string focusMessage { get; set; }

        [FirestoreProperty]
        public double calories { get; set; }

        [FirestoreProperty]
        public double carbs { get; set; }

        [FirestoreProperty]
        public double protein { get; set; }

        [FirestoreProperty]
        public double fat { get; set; }

        [FirestoreProperty]
        public List<string> dos { get; set; }

        [FirestoreProperty]
        public List<string> donts { get; set; }
    }
}
