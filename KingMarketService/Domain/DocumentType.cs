using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KingMarketService.Domain
{
    [DataContract]
    public class DocumentType
    {
        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool OnlyForEnterprise { get; set; }

        [DataMember]
        public int ClassDocumentTypeId { get; set; }
    }
}