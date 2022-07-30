using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    public class FoodServingJson
    {
        public class Serving
        {
            public string calcium { get; set; }
            public string calories { get; set; }
            public string carbohydrate { get; set; }
            public string cholesterol { get; set; }
            public string fat { get; set; }
            public string fiber { get; set; }
            public string iron { get; set; }
            public string measurement_description { get; set; }
            public string metric_serving_amount { get; set; }
            public string metric_serving_unit { get; set; }
            public string monounsaturated_fat { get; set; }
            public string number_of_units { get; set; }
            public string polyunsaturated_fat { get; set; }
            public string potassium { get; set; }
            public string protein { get; set; }
            public string saturated_fat { get; set; }
            public string serving_description { get; set; }
            public string serving_id { get; set; }
            public string serving_url { get; set; }
            public string sodium { get; set; }
            public string sugar { get; set; }
            public string vitamin_a { get; set; }
            public string vitamin_c { get; set; }
        }

        public class Servings
        {
            //[System.Text.Json.Serialization.JsonConverter(typeof(SingleOrArrayConverter<Serving>))]
            public List<Serving> serving { get; set; }
        }

        //public class SingleOrArrayConverter<T> : System.Text.Json.Serialization.JsonConverter<IReadOnlyCollection<T>>
        //{
        //    public override IReadOnlyCollection<T>? Read(
        //    ref Utf8JsonReader reader,
        //    Type typeToConvert,
        //    JsonSerializerOptions options)
        //    => reader.TokenType switch
        //    {
        //        JsonTokenType.StartArray => System.Text.Json.JsonSerializer.Deserialize<T[]>(ref reader, options),
        //        JsonTokenType.StartObject => System.Text.Json.JsonSerializer.Deserialize <Wrapper<T>>(ref reader, options)?.Serving,
        //        _ => throw new System.Text.Json.JsonException()
        //    };


        //    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<T> value, JsonSerializerOptions options)
        //        => System.Text.Json.JsonSerializer.Serialize(writer, (object?)value, options);


        //}

        public class Food
        {
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
    }
}
