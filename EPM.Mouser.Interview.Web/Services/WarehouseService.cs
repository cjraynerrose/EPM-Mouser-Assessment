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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
            {
                
                throw;
            }
        }

        public async Task<List<Product>> GetInStock()
        {
            Func<Product, bool> query = p => 
                p.InStockQuantity > 0
                && p.InStockQuantity > p.ReservedQuantity;

            return await QueryProducts(query);
        }

        public async Task UpdateProductQuantities(Product model)
        {
            try
            {
                await _repository.UpdateQuantities(model);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Attempts to order an item.
        /// <para>
        /// If this takes the reserve above the stock, return false.
        /// A false response means nothing was changed.
        /// </para>
        /// </summary>
        public async Task<bool> OrderItem(Product product, int quantity)
        {
            if(product.InStockQuantity < product.ReservedQuantity + quantity)
            {
                return false;
            }

            product.ReservedQuantity += quantity;
            await UpdateProductQuantities(product);

            return true;
        }

        /// <summary>
        /// Attempts to ship an item.
        /// <para>
        /// If this takes the stock below 0, returns false.
        /// A false response means nothing was changed.
        /// </para>
        /// </summary>
        public async Task<bool> ShipItem(Product product, int quantity)
        {
            if(product.InStockQuantity - quantity < 0)
            {
                return false;
            }

            product.InStockQuantity -= quantity;
            product.ReservedQuantity -= quantity;
            if(product.ReservedQuantity < 0)
            {

                product.ReservedQuantity = 0;
            }

            await UpdateProductQuantities(product);
            return true;
        }

        public async Task RestockItem(Product product, int quantity)
        {
            if(quantity < 0)
            {
                throw new Exception($"Argument cannot be less than 0");
            }

            product.InStockQuantity += quantity;
            await UpdateProductQuantities(product);
        }

        public async Task<Product> InsertProduct(Product product)
        {
            if(product.InStockQuantity < 0)
            {
                throw new Exception($"Argument cannot be less than 0");
            }

            product.Name = await ApplyNameConstraints(product.Name);
            product.ReservedQuantity = 0;

            try
            {
                return await _repository.Insert(product);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private async Task<string> ApplyNameConstraints(string name)
        {
            name = name.Trim();

            // I would prefer to return only the count to save resources.
            var similar = await QueryProducts(p => p.Name.StartsWith(name));
            var count = similar.Count();

            if(count == 0)
            {
                return name;
            }

            return $"{name}({count})";
        }
    }

}
