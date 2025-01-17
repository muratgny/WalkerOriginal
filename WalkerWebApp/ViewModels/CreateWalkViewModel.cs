using WalkerWebApp.Data.Enum;
using WalkerWebApp.Models;

namespace WalkerWebApp.ViewModels
{
    public class CreateWalkViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public WalkCategory WalkCategory { get; set; }
        public string AppUserId { get; set; }
    }
}
