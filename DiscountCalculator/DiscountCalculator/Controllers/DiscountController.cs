using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DiscountCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DiscountController : ControllerBase
    {
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(ILogger<DiscountController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public DiscountResult GetTotalPrice([FromBody] Gold gold)
        {
            double totalPrice = gold.Price.Value * gold.Weight.Value;
            return gold.Discount == 0 ? new DiscountResult(totalPrice) : new DiscountResult(totalPrice - (totalPrice * gold.Discount / 100));
        }
    }
}
