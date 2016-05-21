using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace KingMarketService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISupplierService" in both code and config file together.
    [ServiceContract]
    public interface ISupplierService
    {
        [OperationContract]
        Domain.Supplier GetSupplier(int id);

        [OperationContract]
        List<Domain.Supplier> GetSuppliers();

        [FaultContract(typeof(Exceptions.RepeatedException))]
        [OperationContract]
        Domain.Supplier AddSupplier(Domain.Supplier supplier);

        [OperationContract]
        Domain.Supplier EditSupplier(Domain.Supplier supplier);

        [OperationContract]
        Domain.Supplier DeleteSupplier(int id);
    }
}
