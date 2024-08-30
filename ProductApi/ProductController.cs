using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Interface;
using ProductApi.Services;

namespace ProductApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct productInterface, ProductService productService) : ControllerBase
    {
        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            var products = await productInterface.GetAllAsync();
            if (products.Count != 0)
                return Ok(products);
            return NotFound();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await productInterface.GetAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProduct(Product product)
        {
            var result = await productInterface.UpdateProduct(product);
            return result > 0 ? Ok(result) : BadRequest();
        }


        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var result = await productInterface.AddProduct(product);
            return result > 0 ? Ok(result) : BadRequest();

        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await productInterface.DeleteAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("service")]
        public async Task<IActionResult> GetProductFromService()
        {
            var result = await productService.GetProducts();
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
