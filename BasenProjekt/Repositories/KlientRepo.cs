using Basen.Infrastructure;
using Basen.Models;
using Basen.Pages;
using Microsoft.AspNetCore.Hosting;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Basen.Repositories
{
    public class KlientRepo : IKlientRepo
    {
        private readonly IDocumentStore _documentStore;

        public KlientRepo()
        {
            _documentStore = DocumentStoreHolder.DocumentStore;
        }

        public async Task DodajKlienta(Klient klient)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(klient);
                await session.SaveChangesAsync();
            }
        }

        public async Task EdytujKlienta(Klient klient)
        {
            await Console.Out.WriteLineAsync("Editid: " + klient.Id + "\n");
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(klient);
                await session.SaveChangesAsync();
            }
        }

        public async Task UsunKlienta(string id)
        {
            await Console.Out.WriteLineAsync("Delid: " + id + "\n");
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var klient = await session.LoadAsync<Klient>(id);
                session.Delete(klient);
                await session.SaveChangesAsync();
            }
        }

        public async Task<Klient> PobierzKlienta(string id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<Klient>(id);
            }
        }

        public async Task<List<Klient>> PobierzWszystkichKlientowAsync()
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<Klient>().ToListAsync();
            }
        }

        public async Task<List<Klient>> WyszukajKlientowAsync(string searchTerm)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var query = session.Advanced.AsyncDocumentQuery<Klient>()
                    .Search(x => x.Imie, searchTerm)
                    .Search(x => x.Nazwisko, searchTerm)
                    .Search(x => x.DataUrodzenia, searchTerm);

                var result = await query
                    .WaitForNonStaleResults()
                    .ToListAsync();

                return result;
            }
        }

        public async Task<List<Klient>> PobierzKlientownaStronieAsync(int aktualnastrona, int iloscnastronie)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Advanced.AsyncDocumentQuery<Klient>()
                    .Skip((aktualnastrona - 1) * iloscnastronie)
                .Take(iloscnastronie)
                .WaitForNonStaleResults()
                .ToListAsync();
            }
        }
        








        public async Task<List<Karnet>> PobierzWszystkieKarnetyAsync()
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<Karnet>().ToListAsync();
            }
        }

        public async Task<Karnet> PobierzKarnet(string id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<Karnet>(id);
            }
        }
    }
}
