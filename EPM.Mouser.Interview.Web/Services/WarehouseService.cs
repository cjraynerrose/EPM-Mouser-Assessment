using EPM.Mouser.Interview.Data;
using EPM.Mouser.Interview.Models;

namespace EPM.Mouser.Interview.Web.Services
{
    public class WarehouseService
    {
        private readonly IWarehouseRepository _repository;

        public WarehouseService(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product?> GetProduct(long id)
        {
            try
            {
                return await _repository.Get(id);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                return await _repository.List();
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public async Task<List<Product>> QueryProducts(Func<Product, bool> query)
        {
            try
            {
                return await _repository.Query(query);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public async Task UpdateProductQuantities(Product model)
        {
            try
            {
                await _repository.UpdateQuantities(model);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public async Task<Product> InsertProduct(Product model)
        {
            try
            {
                return await _repository.Insert(model);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }

}
