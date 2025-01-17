using System.ComponentModel.DataAnnotations;

namespace WalkerWebApp.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string? AddressLine { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int PostCode  { get; set; }
    }
}
