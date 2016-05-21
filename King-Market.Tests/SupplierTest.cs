using System;
using System.Text;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace King_Market.Tests
{
    /// <summary>
    /// Summary description for SupplierTest
    /// </summary>
    [TestClass]
    public class SupplierTest
    {
        [TestMethod]
        public void AddSupplierOk()
        {
            ServiceReference1.SupplierServiceClient proxy = new ServiceReference1.SupplierServiceClient();
            var supplier = proxy.AddSupplier(new ServiceReference1.Supplier()
            {
                DocumentTypeId = 2,
                DocumentNumber = "2011111111",
                BusinessName = "Nuevo Proveedor",
                Address = "Direccion AAA",
                Email = "proveedor@gmail.com",
                Web = "",
                Phone = "012222222"
            });
            Assert.AreEqual("2011111111", supplier.DocumentNumber);
            Assert.AreEqual("Nuevo Proveedor", supplier.BusinessName);
        }

        [TestMethod]
        public void AddSupplierRepeated()
        {
            ServiceReference1.SupplierServiceClient proxy = new ServiceReference1.SupplierServiceClient();
            try
            {
                var supplier = proxy.AddSupplier(new ServiceReference1.Supplier()
                {
                    DocumentTypeId = 2,
                    DocumentNumber = "2011111111",
                    BusinessName = "Nuevo Proveedor",
                    Address = "Direccion AAA",
                    Email = "proveedor@gmail.com",
                    Web = "",
                    Phone = "012222222"
                });
            }
            catch (FaultException<ServiceReference1.RepeatedException> error)
            {
                Assert.AreEqual("Error al intentar la creacion", error.Reason.ToString());
                Assert.AreEqual(error.Detail.Id, "101");
                Assert.AreEqual(error.Detail.Description, "El Nro de documento ya existe");
            }
        }

        [TestMethod]
        public void EditSupplierOk()
        {
            ServiceReference1.SupplierServiceClient proxy = new ServiceReference1.SupplierServiceClient();
            var supplier = proxy.EditSupplier(new ServiceReference1.Supplier()
            {
                SupplierId = 8,
                DocumentTypeId = 2,
                DocumentNumber = "2011111111",
                BusinessName = "Proveedor Actualizado",
                Address = "Direccion AAA",
                Email = "proveedor@gmail.com",
                Web = "",
                Phone = "012222222"
            });
            Assert.AreEqual("2011111111", supplier.DocumentNumber);
            Assert.AreEqual("Proveedor Actualizado", supplier.BusinessName);
        }

        [TestMethod]
        public void EditSupplierRepeated()
        {
            ServiceReference1.SupplierServiceClient proxy = new ServiceReference1.SupplierServiceClient();
            try
            {
                var supplier = proxy.EditSupplier(new ServiceReference1.Supplier()
                {
                    SupplierId = 8,
                    DocumentTypeId = 2,
                    DocumentNumber = "20100099528",
                    BusinessName = "Proveedor Actualizado",
                    Address = "Direccion AAA",
                    Email = "proveedor@gmail.com",
                    Web = "",
                    Phone = "012222222"
                });
            }
            catch (FaultException<ServiceReference1.RepeatedException> error)
            {
                Assert.AreEqual("Error al intentar la edicion", error.Reason.ToString());
                Assert.AreEqual(error.Detail.Id, "101");
                Assert.AreEqual(error.Detail.Description, "El Nro de documento ya existe");
            }
        }
    }
}
