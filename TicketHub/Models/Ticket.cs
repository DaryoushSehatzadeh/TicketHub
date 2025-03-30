using System.ComponentModel.DataAnnotations;

namespace TicketHub.Models
{
    public class Ticket
    {
        [Required(ErrorMessage = "Concert ID cannot be empty.")]
        public int concertId { get; set; }


        [EmailAddress(ErrorMessage = "Please enter a valid email address (name@example.com)")]
        [Required(ErrorMessage = "Email cannot be empty.")]
        public String Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Name cannot be empty.")]
        public String Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Phone number cannot be empty.")]
        [Phone(ErrorMessage = "Please enter a valid phone number (xxx-xxx-xxxx)")]
        public String Phone { get; set; } = String.Empty;


        [Required(ErrorMessage = "Quantity cannot be empty.")]
        [Range(1, 10, ErrorMessage = "Number of tickets must be between 1 and 10")] // Cutting the scalpers off
        public int Quantity { get; set; } = 0;


        [CreditCard(ErrorMessage = "Credit card cannot be empty.")]
        [Required(ErrorMessage = "Credit card is required.")]
        public String CreditCard { get; set; } = String.Empty;
        //4417 1234 5678 9113


        [Required(ErrorMessage = "Expiration date cannot be empty.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Expiration must be in MM/YY format.")]
        public String Expiration { get; set; } = String.Empty;


        [Required(ErrorMessage = "Security Code cannot be empty.")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Security Code must be 3 or 4 digits.")]
        public String SecurityCode { get; set; } = String.Empty;

        [Required(ErrorMessage = "Address cannot be empty.")]
        public String Address { get; set; } = String.Empty;

        [Required(ErrorMessage = "City cannot be empty.")]
        public String City { get; set; } = String.Empty;

        [Required(ErrorMessage = "Province cannot be empty.")]
        public String Province { get; set; } = String.Empty;

        [Required(ErrorMessage = "Postal Code is required.")] //I got this regex from the internet, found a great resource at https://regex101.com/
        [RegularExpression(@"(^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$)|(^\d{5}(-\d{4})?$)", ErrorMessage = "Postal Code must be a valid Canadian or US postal code.")]
        public String PostalCode { get; set; } = String.Empty;

        [Required(ErrorMessage = "Country cannot be empty.")]
        public String Country { get; set; } = String.Empty;

    }
}
