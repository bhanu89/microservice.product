using Dapper;
using microservice.product.Data.Cache;
using microservice.product.Data.Entities;
using microservice.product.Extensions;

namespace microservice.product.Data.Repositories
{
    public class CurrencyEntityRepository : IRepository<CurrencyEntity>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly MemCacheRepository<CurrencyEntity> _memCacheRepository;

        public CurrencyEntityRepository(DatabaseContext databaseContext, MemCacheRepository<CurrencyEntity> memCacheRepository)
        {
            _databaseContext = databaseContext;
            _memCacheRepository = memCacheRepository;
        }

        public async Task<IEnumerable<CurrencyEntity>> GetAsync(IEnumerable<long>? Ids)
        {
            var entities = new List<CurrencyEntity>();

            if (Ids.IsNullOrEmpty())
            {
                var query = "SELECT * FROM retail.Currency";

                using (var connection = _databaseContext.GetConnection)
                {
                    var dbEntities = await connection.QueryAsync<CurrencyEntity>(query);
                    entities.AddRange(dbEntities.ToList());
                    entities.ForEach(e => _memCacheRepository.Add(e));
                }
            }
            else
            {
                var idsToFetchFromDB = new List<long>();

                foreach (var Id in Ids)
                {
                    var cachedValue = _memCacheRepository.Get($"{nameof(CurrencyEntity)}-{Id}");
                    if (cachedValue != null)
                    {
                        entities.Add(cachedValue);
                    }
                    else
                    {
                        idsToFetchFromDB.Add(Id);
                    }
                }

                var idsForQuery = idsToFetchFromDB.Distinct().ToArray();

                var query = "SELECT * FROM retail.Currency where Id = ANY(@Ids)";

                using (var connection = _databaseContext.GetConnection)
                {
                    var dbEntities = await connection.QueryAsync<CurrencyEntity>(query, new { Ids = idsForQuery });
                    entities.AddRange(dbEntities.ToList());
                    entities.ForEach(e => _memCacheRepository.Add(e));
                }
            }

            return entities;
        }

        public Task<long> AddAsync(CurrencyEntity value)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(CurrencyEntity value)
        {
            throw new NotImplementedException();
        }
    }
}
