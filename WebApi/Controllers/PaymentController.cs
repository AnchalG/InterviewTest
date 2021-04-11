using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Utilities;
using WebApi.Model;

namespace WebApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        public readonly IProcessGateway _processGateway;

        public PaymentController(IProcessGateway processGateway)
        {
            _processGateway = processGateway;
        }

        [HttpPost]
        
        public IActionResult ProcessPayment(PaymentModel payment)
        {
            try
            {
                var  paymentState = _processGateway.ProceessAmount(payment);
                PaymentStatus status;

                if (Enum.TryParse<PaymentStatus>(paymentState.StateOfTransation, out status)) {

                    if (status == PaymentStatus.Processed) {
                        return Ok("Transaltion Id: " + paymentState.TransactionId + " was completed successfully.");
                    }
                    else
                    {
                        return StatusCode(500, "Transaltion Id: " + paymentState.TransactionId + " failed. Please try again later.");
                    }
                }
                else return StatusCode(500, new ObjectResult("Something went wrong. Please try again later."));

            }
            catch (Exception ex) {

                return StatusCode(500, new ObjectResult("Something went wrong. Please try again later."));
            }
        }


    }
}
