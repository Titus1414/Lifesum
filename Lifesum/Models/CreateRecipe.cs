using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class CreateRecipe
    {
        [FirestoreDocumentId]
        public string recipeId { get; set; }

        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string title { get; set; }

        [FirestoreProperty]
        public double servings { get; set; }

        [FirestoreProperty]
        public string image { get; set; }

        [FirestoreProperty]
        public List<string> food { get; set; }

        [FirestoreProperty]
        public List<string> instructions { get; set; }

        [FirestoreProperty]
        public int preparingTime { get; set; }

        [FirestoreProperty]
        public List<string> manualIngredients { get; set; }

        [FirestoreProperty]
        public List<string> filter { get; set; }

        [FirestoreProperty]
        public string type { get; set; }
    }
}
