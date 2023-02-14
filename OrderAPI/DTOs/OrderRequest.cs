using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrderAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.DTOs
{
    public class OrderRequest
    {
        [Required, OrderAmount(1, 999)]
        public string OrderAmount { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required, ExpectedDeliveryDate]
        public string ExpectedDeliveryDate { get; set; }
    }
}
