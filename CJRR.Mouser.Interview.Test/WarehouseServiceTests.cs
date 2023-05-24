using EPM.Mouser.Interview.Data;
using EPM.Mouser.Interview.Models;
using EPM.Mouser.Interview.Web.Services;

namespace CJRR.Mouser.Interview.Tests
{
    [TestFixture]
    public class WarehouseServiceTests
    {
        private WarehouseService _warehouseService;
        private IWarehouseRepository _warehouseRepository;

        [SetUp]
        public void SetUp()
        {
            _warehouseRepository = new WarehouseRepository();
            _warehouseService = new WarehouseService(_warehouseRepository);
        }


        private async Task<int> GetProductCount()
            => (await _warehouseRepository.Query(p => p.Id != -1)).Count;

        [Test]
        public async Task GetProduct_ExistingId_ReturnsProduct()
        {
            // Arrange
            var productId = 1;

            // Act
            var product = await _warehouseService.GetProduct(productId);

            // Assert
            Assert.That(product, Is.Not.Null);
            Assert.That(product.Id, Is.EqualTo(productId));
        }

        [Test]
        public async Task GetProduct_NonExistingId_ReturnsNull()
        {
            // Arrange
            var productId = -1;

            // Act
            var product = await _warehouseService.GetProduct(productId);

            // Assert
            Assert.That(product, Is.Null);
        }

        [Test]
        public async Task GetAllProducts_ReturnsAllProducts()
        {
            // Act
            var products = await _warehouseService.GetAllProducts();

            // Assert
            Assert.That(products, Is.Not.Null);
            Assert.That(products.Count, Is.EqualTo(await GetProductCount()));
        }

        [Test]
        public async Task QueryProducts_ReturnsMatchingProducts()
        {
            // Arrange
            Func<Product, bool> query = p => p.InStockQuantity > 0;

            // Act
            var products = await _warehouseService.QueryProducts(query);

            // Assert
            Assert.That(products, Is.Not.Null);
            Assert.That(products.All(query), Is.True);
        }

        [Test]
        public async Task UpdateProductQuantities_ValidModel_NoExceptionsThrown()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", InStockQuantity = 10, ReservedQuantity = 5 };

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _warehouseService.UpdateProductQuantities(product));
        }

        [Test]
        public async Task InsertProduct_ValidModel_ReturnsInsertedProduct()
        {
            // Arrange
            var product = new Product { Name = "New Product", InStockQuantity = 5, ReservedQuantity = 0 };

            // Act
            var insertedProduct = await _warehouseService.InsertProduct(product);

            // Assert
            Assert.That(insertedProduct, Is.Not.Null);
            Assert.That(insertedProduct.Name, Is.EqualTo(product.Name));
            Assert.That(insertedProduct.InStockQuantity, Is.EqualTo(product.InStockQuantity));
            Assert.That(insertedProduct.ReservedQuantity, Is.EqualTo(product.ReservedQuantity));
            Assert.That(insertedProduct.Id, Is.GreaterThan(0));
        }

        [Test]
        // An "Invalid" model is still accepted. The ReservedQuanity is changed to 0.
        public async Task InsertProduce_InvalidModel_StillWorks()
        {
            // Arrange
            var product = new Product { Name = "", InStockQuantity = -5, ReservedQuantity = -10 };

            // Act
            var insertedProduct = await _warehouseService.InsertProduct(product);

            // Assert
            Assert.That(insertedProduct, Is.Not.Null);
            Assert.That(insertedProduct.Name, Is.EqualTo(product.Name));
            Assert.That(insertedProduct.InStockQuantity, Is.EqualTo(product.InStockQuantity));
            Assert.That(insertedProduct.ReservedQuantity, Is.Not.EqualTo(product.ReservedQuantity));
            Assert.That(insertedProduct.Id, Is.GreaterThan(0));
        }


    }
}
