﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    public class SingleObjectJson
    {
        public class Food
        {
            public string brand_name { get; set; }
            public string food_id { get; set; }
            public string food_name { get; set; }
            public string food_type { get; set; }
            public string food_url { get; set; }
            public Servings servings { get; set; }
        }

        public class Root
        {
            public Food food { get; set; }
        }

        public class Serving
        {
            public string calories { get; set; }
            public string carbohydrate { get; set; }
            public string cholesterol { get; set; }
            public string fat { get; set; }
            public string fiber { get; set; }
            public string measurement_description { get; set; }
            public string metric_serving_amount { get; set; }
            public string metric_serving_unit { get; set; }
            public string number_of_units { get; set; }
            public string potassium { get; set; }
            public string protein { get; set; }
            public string saturated_fat { get; set; }
            public string serving_description { get; set; }
            public string serving_id { get; set; }
            public string serving_url { get; set; }
            public string sodium { get; set; }
            public string sugar { get; set; }
        }

        public class Servings
        {
            public Serving serving { get; set; }
        }
    }
}
