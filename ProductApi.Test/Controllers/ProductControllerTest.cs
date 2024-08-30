using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Test.Controllers
{
    public class ProductControllerTest : IClassFixture<ProductWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public ProductControllerTest(ProductWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // GetProduct -OK
        [Fact]
        public async Task GetProduct_ReturnsOkResult_WithListOfProducts()
        {
            // Act: Make a GET request to the Products API
            var response = await _client.GetAsync("/api/Product");

            // Assert: Ensure the request was successful and returned products
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString);
            Assert.NotEmpty(products!);
        }

        // Get Products -Not found
        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenNoProductsExist()
        {
            // Act: Make a GET request to the Products API when no products exist
            var response = await _client.GetAsync("/api/Product");

            // Assert: Ensure the request returns a 404 Not Found status
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetProductById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange: Seed a product into the database
            var productId = 1;

            // Act: Make a GET request to the Products API with a valid product ID
            var response = await _client.GetAsync($"/api/Product/{productId}");

            // Assert: Ensure the request was successful and returned the correct product
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(responseString);
            Assert.Equal(productId, product!.Id);
        }

        [Fact]
        public async Task GetProductById_ReturnsNotFound_ForInvalidProductId()
        {
            // Act: Make a GET request to the Products API with an invalid product ID
            var response = await _client.GetAsync("/api/Products/999");

            // Assert: Ensure the request returns a 404 Not Found status
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostProduct_ReturnsOkResult_WhenProductIsCreated()
        {
            // Arrange: Create a new product object
            var product = new Product { Name = "Test Product" };
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            // Act: Make a POST request to the Products API
            var response = await _client.PostAsync("/api/Product", content);

            // Assert: Ensure the request was successful and the product was created
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            int createdProductId = int.Parse(responseString);
            createdProductId.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOkResult_WhenProductIsUpdated()
        {
            // Arrange: Create and update a product
            var product = new Product { Id = 1, Name = "Updated Product" };
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            // Act: Make a PUT request to update the product
            var response = await _client.PutAsync("/api/Product", content);

            // Assert: Ensure the request was successful and the product was updated
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            int createdProductId = int.Parse(responseString);
            createdProductId.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsOkResult_WhenProductIsDeleted()
        {
            // Arrange: Seed a product and then delete it
            var productId = 1;

            // Act: Make a DELETE request to the Products API
            var response = await _client.DeleteAsync($"/api/Product/{productId}");

            // Assert: Ensure the request was successful and the product was deleted
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            int createdProductId = int.Parse(responseString);
            createdProductId.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task GetProductFromService_ReturnsOkResult_WithProductsFromService()
        {
            // Act: Make a GET request to the Products Service API
            var response = await _client.GetAsync("/api/Product/service");

            // Assert: Ensure the request was successful and returned products from the service
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString);
            Assert.NotEmpty(products!);
        }
    }
}
