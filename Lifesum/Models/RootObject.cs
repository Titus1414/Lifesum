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
    public class RootObject
    {
        [DataMember]
        public string access_token { get; set; }

        [DataMember]
        public string expires_in { get; set; }
        
        [DataMember]
        public string token_type { get; set; }

        [DataMember]
        public string scope { get; set; }
    }
}
