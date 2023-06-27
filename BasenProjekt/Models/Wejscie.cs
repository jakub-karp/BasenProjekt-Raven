

using Raven.Client.Documents.Indexes;

namespace Basen.Models
{

    public class Wejscie
    {
        public string? Id { get; set; }
        public string NumerSzafki { get; set; }
        public Klient? Klient { get; set; } // Dodane pole Klient
        public DateTime CzasRozpoczecia { get; set; }
        public DateTime CzasZakonczenia { get; set; }
        public string? klientId { get; set; }

        public string KlientImieNazwisko => Klient != null ? $"{Klient.Imie} {Klient.Nazwisko}" : "";
    }

    public class WejscieTime : AbstractIndexCreationTask<Wejscie>
    {
        public WejscieTime() {
            Map = wejscia => from wejscie in wejscia
                             select new
                             {
                                 wejscie.CzasZakonczenia
                             };
            Index(x => x.CzasZakonczenia, FieldIndexing.Search);
        }
    }
}

