using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using Raven.Client.Documents.Indexes;
using Basen.Models;

namespace Basen.Infrastructure
{
    public static class DocumentStoreHolder
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
       new Lazy<IDocumentStore>(() =>
       {
           var store = new DocumentStore
           {
               Urls = new[] { "http://localhost:8080" },
               Database = "BasenDB"
           };

           store.Initialize();

           IndexCreation.CreateIndexes(typeof(KarnetTime).Assembly, store);
           IndexCreation.CreateIndexes(typeof(WejscieTime).Assembly, store);

           var databaseRecord = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));

           if (databaseRecord != null)
               return store;

           var createDatabaseOperation =
               new CreateDatabaseOperation(new DatabaseRecord(store.Database));

           store.Maintenance.Server.Send(createDatabaseOperation);


           return store;
       });

        public static IDocumentStore DocumentStore =>
            LazyStore.Value;
    }
}
