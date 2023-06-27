using Basen.Infrastructure;
using Basen.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace BasenProjekt.Repositories
{
    public class WejsciaRepo : IWejsciaRepo
    {

        
        private readonly IDocumentStore _documentStore;
        public WejsciaRepo() {
            _documentStore = DocumentStoreHolder.DocumentStore;

        }

        public async Task DodajWejscie(Wejscie wejscie)
        {
            using(IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(wejscie);
                await session.SaveChangesAsync();
            }
        }

        public async Task EdytujWejscie(Wejscie wejscie)
        {
            using(IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(wejscie);
                await session.SaveChangesAsync();
            }
        }

        public async Task UsunWejscie(string id)
        {
            using(IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var wejscie = await session.LoadAsync<Wejscie>(id);
                session.Delete(wejscie);
                await session.SaveChangesAsync();
            }
        }

        public async Task<Wejscie> PobierzWejscie(string id)
        {
            using(IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<Wejscie>(id);
            }
        }

        public async Task<List<Wejscie>> PobierzWszystkieWejsciaAsync()
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<Wejscie>().ToListAsync();
            }
        }

        public async Task<List<Wejscie>> WyszukajWejsciaAsync(string searchTerm)
        {
            using( var session = _documentStore.OpenAsyncSession())
            {
                var query = session.Advanced.AsyncDocumentQuery<Wejscie>()
                    .Search(x => x.NumerSzafki, searchTerm)
                    .Search(x => x.KlientImieNazwisko, searchTerm);

                var result = await query
                    .WaitForNonStaleResults()
                    .ToListAsync();

                return result;
            }
        }
        public async Task<List<Klient>> PobierzWszystkichKlientowAsync()
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<Klient>().ToListAsync();
            }
        }
        public async Task<Klient> PobierzKlienta(string id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<Klient>(id);
            }
        }

        public async Task<List<Wejscie>> ZakonczoneAsync()
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var karnety = await session.Query<Wejscie, WejscieTime>()
                    .Where(x => x.CzasZakonczenia < DateTime.Now)
                    .ToListAsync();

                return karnety;
            }
        }

    }
}
