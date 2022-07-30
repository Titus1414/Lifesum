using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;


namespace Lifesum.Models
{
    [FirestoreData]
    public class Plans
    {
        [FirestoreDocumentId]
        public string plansId { get; set; }

        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public string aim { get; set; }
        [FirestoreProperty]
        public string color1 { get; set; }
        [FirestoreProperty]
        public string color2 { get; set; }
        [FirestoreProperty]
        public string description { get; set; }
        [FirestoreProperty]
        public string testimonial { get; set; }
        [FirestoreProperty]
        public string testimonialBy { get; set; }
        [FirestoreProperty]
        public string testimonialSpecialization { get; set; }
        [FirestoreProperty]
        public string type { get; set; }

        [FirestoreProperty]
        public string warningMessage { get; set; }

        [FirestoreProperty]
        public List<string> goal{ get; set; }

        [FirestoreProperty]
        public string coverImage { get; set; }

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

        [FirestoreProperty]
        public List<string> features { get; set; }

        //[FirestoreProperty]
        //public List<string> recipes { get; set; }
    }
}
