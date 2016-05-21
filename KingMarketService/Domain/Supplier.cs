using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KingMarketService.Domain
{
    [DataContract]
    public class Supplier
    {
        [DataMember]
        public int SupplierId { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string BusinessName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Web { get; set; }

        [DataMember]
        public string Phone { get; set; }
    }
}