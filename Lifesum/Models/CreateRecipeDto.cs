using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [FirestoreData]
    public class CreateRecipeDto
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
        public List<string> instructions { get; set; }

        [FirestoreProperty]
        public List<string> manualIngredients { get; set; }

        [FirestoreProperty]
        public int preparingTime { get; set; }

        [FirestoreProperty]
        public List<string> filter { get; set; }

        [FirestoreProperty]
        public string type { get; set; }

        [FirestoreProperty]
        public List<string> titleSearch { get; set; }

        [FirestoreProperty]
        public List<string> food { get; set; }

        [FirestoreProperty]
        public double calcium { get; set; }

        [FirestoreProperty]
        public double calories { get; set; }

        [FirestoreProperty]
        public double carbohydrate { get; set; }

        [FirestoreProperty]
        public double cholesterol { get; set; }

        [FirestoreProperty]
        public double fat { get; set; }

        [FirestoreProperty]
        public double fiber { get; set; }

        [FirestoreProperty]
        public double iron { get; set; }

        [FirestoreProperty]
        public double monounsaturated_fat { get; set; }

        [FirestoreProperty]
        public double polyunsaturated_fat { get; set; }

        [FirestoreProperty]
        public double potassium { get; set; }

        [FirestoreProperty]
        public double protein { get; set; }

        [FirestoreProperty]
        public double saturated_fat { get; set; }

        [FirestoreProperty]
        public double sodium { get; set; }

        [FirestoreProperty]
        public double sugar { get; set; }

        [FirestoreProperty]
        public double vitamin_a { get; set; }

        [FirestoreProperty]
        public double vitamin_c { get; set; }

        [FirestoreProperty]
        public List<string>  plans { get; set; }

        [FirestoreProperty]
        public List<FoodApiDtoForRecipe> foods { get; set; }
    }
}
