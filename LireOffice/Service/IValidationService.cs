using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Service
{
    public interface IValidationService
    {
        bool ValidateVendorRegisterId(string registerId, out ICollection<string> validationErrors);

    }
}
