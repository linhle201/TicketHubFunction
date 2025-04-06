using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHubFunction
{
    public class Tickethub
    {

        [Required]
        public int ConcertId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Phone]
        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [CreditCard]
        public string CreditCard { get; set; } = string.Empty;


        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Invalid expiration date format. Use MM/YY.")]
        public string Expiration { get; set; } = string.Empty;


        [Required]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid security code. Must be 3 digits.")]
        public string SecurityCode { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Province { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"(^\d{5}(-\d{4})?$)|(^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$)", ErrorMessage = "Invalid postal code format.")]
        public string PostalCode { get; set; } = string.Empty;
       

        [Required]
        public string Country { get; set; } = string.Empty;
    }
}
