using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KingMarketService.Exceptions;
using KingMarketService.Persistence;

namespace KingMarketService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SupplierService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SupplierService.svc or SupplierService.svc.cs at the Solution Explorer and start debugging.
    public class SupplierService : ISupplierService
    {
        public Domain.Supplier GetSupplier(int id)
        {
            var db = new KingMarketEntities();
            var supplier = db.Suppliers.Find(id);
            db.Dispose();
            return new Domain.Supplier
            {
                SupplierId = supplier.SupplierId,
                DocumentTypeId = supplier.DocumentTypeId,
                DocumentNumber = supplier.DocumentNumber,
                BusinessName = supplier.BusinessName,
                Address = supplier.Address,
                Email = supplier.Email,
                Web = supplier.Web,
                Phone = supplier.Phone
            };
        }

        public List<Domain.Supplier> GetSuppliers()
        {
            var db = new KingMarketEntities();
            var suppliers = db.Suppliers.ToList();
            db.Dispose();
            var suppliersDto = new List<Domain.Supplier>();
            foreach (var item in suppliers)
            {
                var supplier = new Domain.Supplier
                {
                    SupplierId = item.SupplierId,
                    DocumentTypeId = item.DocumentTypeId,
                    DocumentNumber = item.DocumentNumber,
                    BusinessName = item.BusinessName,
                    Address = item.Address,
                    Email = item.Email,
                    Web = item.Web,
                    Phone = item.Phone
                };
                suppliersDto.Add(supplier);
            }
            return suppliersDto;
        }

        public Domain.Supplier AddSupplier(Domain.Supplier supplier)
        {
            var db = new KingMarketEntities();
            var su = db.Suppliers.ToList().Find(s => s.DocumentNumber.Equals(supplier.DocumentNumber));
            if (su != null)
            {
                db.Dispose();
                throw new FaultException<RepeatedException>(new RepeatedException()
                {
                    Id = "101",
                    Description = "El Nro de documento ya existe"
                }, new FaultReason("Error al intentar la creacion"));
            }
            su = new Supplier
            {
                DocumentTypeId = supplier.DocumentTypeId,
                DocumentNumber = supplier.DocumentNumber,
                BusinessName = supplier.BusinessName,
                Address = supplier.Address,
                Email = supplier.Email,
                Web = supplier.Web,
                Phone = supplier.Phone
            };
            db.Suppliers.Add(su);
            db.SaveChanges();
            db.Dispose();
            supplier.SupplierId = su.SupplierId;
            return supplier;
        }

        public Domain.Supplier EditSupplier(Domain.Supplier supplier)
        {
            var db = new KingMarketEntities();
            var su = db.Suppliers.ToList().Find(s => s.DocumentNumber.Equals(supplier.DocumentNumber));
            if (su != null)
            {
                db.Dispose();
                throw new FaultException<RepeatedException>(new RepeatedException()
                {
                    Id = "101",
                    Description = "El Nro de documento ya existe"
                }, new FaultReason("Error al intentar la edicion"));
            }
            su = new Supplier
            {
                SupplierId = supplier.SupplierId,
                DocumentTypeId = supplier.DocumentTypeId,
                DocumentNumber = supplier.DocumentNumber,
                BusinessName = supplier.BusinessName,
                Address = supplier.Address,
                Email = supplier.Email,
                Web = supplier.Web,
                Phone = supplier.Phone
            };
            db.Entry(su).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return supplier;
        }

        public Domain.Supplier DeleteSupplier(int id)
        {
            var db = new KingMarketEntities();
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            db.Dispose();
            return new Domain.Supplier
            {
                SupplierId = supplier.SupplierId,
                DocumentTypeId = supplier.DocumentTypeId,
                DocumentNumber = supplier.DocumentNumber,
                BusinessName = supplier.BusinessName,
                Address = supplier.Address,
                Email = supplier.Email,
                Web = supplier.Web,
                Phone = supplier.Phone
            };
        }
    }
}
