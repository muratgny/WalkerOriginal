using WalkerWebApp.Data.Enum;
using WalkerWebApp.Models;

namespace WalkerWebApp.ViewModels
{
    public class EditWalkViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public WalkCategory WalkCategory { get; set; }
    }
}
