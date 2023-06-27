using Basen.Infrastructure;
using Basen.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace BasenProjekt.Repositories
{
    public class KarnetRepo : IKarnetRepo
    {
        private readonly IDocumentStore _documentStore;
        private readonly ILogger<KarnetRepo> _logger;

        public KarnetRepo(ILogger<KarnetRepo> logger)
        {
            _documentStore = DocumentStoreHolder.DocumentStore;
            _logger = logger;
        }

        public async Task DodajKarnet(Karnet karnet)
        {
            _logger.LogInformation("Próba dodania nowego karnetu: {@karnet}", karnet);

            try
            {
                using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
                {
                    await session.StoreAsync(karnet);
                    await session.SaveChangesAsync();
                }

                _logger.LogInformation("Pomyślnie dodano karnet: {@karnet}", karnet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas dodawania karnetu: {@karnet}", karnet);
                throw; // Przekazujemy wyjątek dalej, aby informacje o błędzie były dostępne na wyższych poziomach
            }
        }

        public async Task EdytujKarnet(Karnet karnet)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(karnet);
                await session.SaveChangesAsync();
            }
        }

        public async Task UsunKarnet(string id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var karnet = await session.LoadAsync<Karnet>(id);
                session.Delete(karnet);
                await session.SaveChangesAsync();
            }
        }

        public async Task<Karnet> PobierzKarnet(string id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<Karnet>(id);
            }
        }

        public async Task<List<Karnet>> PobierzWszystkieKarnetyAsync()
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<Karnet>().ToListAsync();
            }
        }

        public async Task<List<Karnet>> WyszukajKarnetyAsync(string searchTerm)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var query = session.Advanced.AsyncDocumentQuery<Karnet>()
                    .Search(x => x.Numer, searchTerm)
                    .Search(x => x.DataRozpoczecia, searchTerm)
                    .Search(x => x.DataZakonczenia, searchTerm);

                var result = await query
                    .WaitForNonStaleResults()
                    .ToListAsync();

                return result;
            }
        }

        public async Task<List<Karnet>> ZakonczoneAsync()
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var karnety = await session.Query<Karnet, KarnetTime>()
                    .Where(x => x.DataZakonczenia < DateTime.Now)
                    .ToListAsync();

                return karnety;
            }
        }



    }
}