using Raven.Client.Documents.Indexes;

namespace Basen.Models
{
    public class Karnet
    {
        public string Id { get; set; }
        public string Numer { get; set; }
        public DateTime DataRozpoczecia { get; set; }
        public DateTime DataZakonczenia { get; set; }

        public bool Search(string searchTerm)
        {
            return Numer.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                   DataRozpoczecia.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                   DataZakonczenia.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class KarnetTime : AbstractIndexCreationTask<Karnet>
    {
        public KarnetTime()
        {
            Map = karnety => from karnet in karnety
                           select new
                           {
                               karnet.DataZakonczenia
                           };
            Index(x => x.DataZakonczenia, FieldIndexing.Search);
        }
    }
}
