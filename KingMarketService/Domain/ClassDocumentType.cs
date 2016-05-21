using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KingMarketService.Domain
{
    [DataContract]
    public class ClassDocumentType
    {
        [DataMember]
        public int ClassDocumentTypeId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}