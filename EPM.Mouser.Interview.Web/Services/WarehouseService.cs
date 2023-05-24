﻿using EPM.Mouser.Interview.Data;
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

        public async Task<Product> InsertProduct(Product model)
        {
            try
            {
                return await _repository.Insert(model);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }

}
