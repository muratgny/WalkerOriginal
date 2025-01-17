using WalkerWebApp.Models;

namespace WalkerWebApp.ViewModels
{
    public class ListClubByCountryViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
        public bool NoClubWarning { get; set; } = false;
        public string Country { get; set; }
    }
}
