using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Lifesum.Models
{
    [DataContract]
    public class foodsearch
    {
        [DataMember]
        public string foodId { get; set; }

        [DataMember]
        public string foodDescription { get; set; }

        [DataMember]
        public string foodName { get; set; }

        [DataMember]
        public string foodType { get; set; }

        [DataMember]
        public string foodUrl { get; set; }
    }
}
