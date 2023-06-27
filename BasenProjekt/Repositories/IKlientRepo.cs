using Basen.Models;
using System.Collections.Generic;

namespace Basen.Repositories
{
    public interface IKlientRepo
    {
        Task<List<Klient>> PobierzWszystkichKlientowAsync();

        Task DodajKlienta(Klient klient);

        Task EdytujKlienta(Klient klient);

        Task UsunKlienta(string id);

        Task<Klient> PobierzKlienta(string id);
        Task<List<Klient>> WyszukajKlientowAsync(string searchTerm);


        Task<List<Karnet>> PobierzWszystkieKarnetyAsync();

        Task<Karnet> PobierzKarnet(string id);

        Task<List<Klient>> PobierzKlientownaStronieAsync(int aktualnastrona, int iloscnastronie);
    }
}
