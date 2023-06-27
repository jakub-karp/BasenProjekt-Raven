namespace Basen.Models
{
    public class Klient
    {
        public string? Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public string? KarnetId { get; set; }
        public Karnet? Karnet { get; set; }

        public bool Search(string searchTerm)
        {
            return Imie.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                   Nazwisko.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                   DataUrodzenia.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
        }
    }
}
