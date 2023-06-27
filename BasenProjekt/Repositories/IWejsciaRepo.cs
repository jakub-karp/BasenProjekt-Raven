using Basen.Models;
using Raven.Client.Documents.Session;

namespace BasenProjekt.Repositories
{
    public interface IWejsciaRepo
    {
        Task DodajWejscie(Wejscie wejscie);

        Task EdytujWejscie(Wejscie wejscie);

        Task UsunWejscie(string id);

        Task<Wejscie> PobierzWejscie(string id);

        Task<List<Wejscie>> PobierzWszystkieWejsciaAsync();


        Task<List<Wejscie>> WyszukajWejsciaAsync(string searchTerm);
        Task<List<Klient>> PobierzWszystkichKlientowAsync();
        Task<Klient> PobierzKlienta(string id);
        Task<List<Wejscie>> ZakonczoneAsync();
    }
}
