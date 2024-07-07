using Dapper;
using microservice.product.Data.Cache;
using microservice.product.Data.Entities;
using microservice.product.Extensions;

namespace microservice.product.Data.Repositories
{
    public class ProductEntityRepository : IRepository<ProductEntity>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly MemCacheRepository<ProductEntity> _memCacheRepository;

        private const int CacheExpirationTimeInMinutes = 10;

        public ProductEntityRepository(DatabaseContext databaseContext, MemCacheRepository<ProductEntity> memCacheRepository)
        {
            _databaseContext = databaseContext;
            _memCacheRepository = memCacheRepository;
        }

        public async Task<long> AddAsync(ProductEntity value)
        {
            long insertedId;
            
            var command = @"INSERT INTO retail.Product (Name, Brand, Price, CurrencyCodeId)
VALUES (@Name, @Brand, @Price, @CurrencyCodeId) RETURNING Id";
            
            using (var connection = _databaseContext.GetConnection)
            {
                insertedId = await connection.ExecuteScalarAsync<long>(command, value);
            }
            value.Id = insertedId;
            _memCacheRepository.Add(value);

            return insertedId;
        }

        public async Task<int> UpdateAsync(ProductEntity value)
        {
            int rowsAffected;

            var command = @"UPDATE retail.Product SET Name = @Name, Brand = @Brand, Price = @Price, 
CurrencyCodeId = @CurrencyCodeId WHERE Id = @Id AND Version = @Version";

            using (var connection = _databaseContext.GetConnection)
            {
                rowsAffected = await connection.ExecuteAsync(command, value);
            }

            if (rowsAffected == 0)
            {
                throw new Exception($"Failed to update product with Id {value.Id} and Version {value.Version}");
            }

            _memCacheRepository.Add(value, TimeSpan.FromMinutes(CacheExpirationTimeInMinutes));
            return rowsAffected;
        }

        public async Task DeleteAsync(long id)
        {
            var command = "UPDATE retail.Product SET Deleted = TRUE WHERE Id = @Id";

            using (var connection = _databaseContext.GetConnection)
            {
                await connection.ExecuteAsync(command, new { Id = id });
            }

            _memCacheRepository.Delete($"{nameof(ProductEntity)}-{id}");
        }

        public async Task<IEnumerable<ProductEntity>> GetAsync(IEnumerable<long>? Ids)
        {
            var entities = new List<ProductEntity>();

            if (Ids.IsNullOrEmpty())
            {
                var query = "SELECT * FROM retail.Product WHERE Deleted = FALSE ORDER BY Id ASC";

                using (var connection = _databaseContext.GetConnection)
                {
                    var dbEntities = await connection.QueryAsync<ProductEntity>(query);
                    entities.AddRange(dbEntities.ToList());
                }
            }
            else
            {
                var idsToFetchFromDB = new List<long>();

                foreach (var Id in Ids)
                {
                    var cachedValue = _memCacheRepository.Get($"{nameof(ProductEntity)}-{Id}");
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

                var query = "SELECT * FROM retail.Product WHERE Id = ANY(@Ids) AND Deleted = FALSE ORDER BY Id ASC";

                using (var connection = _databaseContext.GetConnection)
                {
                    var dbEntities = await connection.QueryAsync<ProductEntity>(query, new { Ids = idsForQuery });
                    entities.AddRange(dbEntities.ToList());
                    entities.ForEach(e => _memCacheRepository.Add(e, TimeSpan.FromMinutes(CacheExpirationTimeInMinutes)));
                }
            }

            return entities;
        }
    }
}
