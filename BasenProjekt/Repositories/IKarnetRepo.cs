using Basen.Infrastructure;
using Basen.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace BasenProjekt.Repositories
{
    public interface IKarnetRepo
    {

        Task DodajKarnet(Karnet karnet);

        Task EdytujKarnet(Karnet karnet);


        Task UsunKarnet(string id);


        Task<Karnet> PobierzKarnet(string id);


        Task<List<Karnet>> PobierzWszystkieKarnetyAsync();


        Task<List<Karnet>> WyszukajKarnetyAsync(string searchTerm);
        Task<List<Karnet>> ZakonczoneAsync();
    }
    
}
