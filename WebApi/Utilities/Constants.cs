using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Utilities
{

    public enum PaymentStatus
    {
        Pending, Processed, Failed
    }

    public enum GatewayStatus
    {
        Available, NotAvailable
    }
}
