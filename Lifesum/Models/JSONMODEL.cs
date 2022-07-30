using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    public class JSONMODEL
    {
        public class Rootobject
        {
            public Foods foods { get; set; }
        }

        public class Foods
        {
            public Food[] food { get; set; }
            public string max_results { get; set; }
            public string page_number { get; set; }
            public string total_results { get; set; }
        }

        public class Food
        {
            public string food_description { get; set; }
            public string food_id { get; set; }
            public string food_name { get; set; }
            public string food_type { get; set; }
            public string food_url { get; set; }
            public string brand_name { get; set; }
        }

    }
}
