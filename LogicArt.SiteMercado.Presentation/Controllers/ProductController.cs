using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicArt.Arch.Application.Repositories.Abstractions.Transaction;
using LogicArt.Arch.Infrastructure.Persistence.EntityFramework;
using LogicArt.SiteMercado.Application.Adapters.Abstractions;
using LogicArt.SiteMercado.Application.Data;
using LogicArt.SiteMercado.Core.Services.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;
using LogicArt.SiteMercado.Presentation.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LogicArt.SiteMercado.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProductController : ControllerBase
    {
        private readonly IAdapter<Product, ProductDTO> _productAdapter;
        private readonly IProductService _productService;

        public ProductController(IAdapter<Product, ProductDTO> productAdapter, IProductService productService)
        {
            _productAdapter = productAdapter;
            _productService = productService;
        }


        [HttpDelete("{productId}")]
        [DbTransaction]
        public async Task<IActionResult> Delete(Guid productId)
        {
            var product = await _productService.FindByIdAsync(productId);
            if (product == null)
            {
                return this.NotFound();
            }

            _productService.Remove(product);

            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDTO resource)
        {
            var product = _productAdapter.Adapt(resource);
            await _productService.AddAsync(product);

            var unitOfWork = (EntityFrameworkUnitOfWork)this.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Context.SaveChangesAsync();

            var response = new { ResourceId = product.Id, product.Version };
            return this.CreatedAtRoute(new { id = response.ResourceId }, response);
        }

        [HttpPatch("{productId}")]
        public async Task<IActionResult> Update(Guid productId, [FromBody] JsonPatchDocument<Product> patchResource)
        {
            var product = await _productService.FindByIdAsync(productId);
            if (product == null)
            {
                return this.NotFound();
            }

            patchResource.ApplyTo(product);

            var unitOfWork = (EntityFrameworkUnitOfWork)this.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Context.SaveChangesAsync();

            var response = new { ResourceId = product.Id, product.Version };
            return this.Ok(response);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(Guid productId)
        {
            var product = await _productService.FindByIdAsync(productId);
            if (product == null)
            {
                return this.NotFound();
            }

            var response = _productAdapter.Adapt(product);
            return this.Ok(response);
        }

        [HttpGet]
        public async IAsyncEnumerable<ProductDTO> GetProducts([FromQuery] string? name)
        {
            var products = string.IsNullOrEmpty(name) ? _productService.GetAllAsync() : _productService.GetProductsByNameAsync(name);
            await foreach (var product in products)
            {
                yield return _productAdapter.Adapt(product);
            }
        }
    }
}
