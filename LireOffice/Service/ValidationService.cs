using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Service
{
    public class ValidationService : IValidationService
    {
        private readonly IOfficeContext context;

        public ValidationService(IOfficeContext context)
        {
            this.context = context;
        }

        public bool ValidateVendorRegisterId(string registerId, out ICollection<string> validationErrors)
        {
            validationErrors = new List<string>();
            
            var user = context.GetVendor().AsQueryable().Where(c => string.Equals(c.RegisterId, registerId)).ToList();
            if (user.Count > 0)
                validationErrors.Add("No. Register sudah terpakai, silahkan gunakan nomor yang lain.");

            return validationErrors.Count == 0;
        }
    }
}
