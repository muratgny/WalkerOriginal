using WalkerWebApp.Models;

namespace WalkerWebApp.ViewModels
{
    public class ListClubByCityViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
        public bool NoClubWarning { get; set; } = false;
        public string City { get; set; }
        public string Country { get; set; }
    }
}
