using microservice.product.Application.CommandHandlers;
using microservice.product.Application.Commands;
using microservice.product.Application.Queries;
using microservice.product.Application.QueryHandlers;
using microservice.product.Extensions;
using microservice.product.Models;
using Microsoft.AspNetCore.Mvc;

namespace microservice.product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AddProductCommandHandler _addProductCommandHandler;
        private readonly GetProdcutsQueryHandler _getProductsQueryHandler;
        private readonly UpdateProductCommandHandler _updateProductCommandHandler;
        private readonly DeleteProductCommandHandler _deleteProductCommandHandler;

        public ProductController(AddProductCommandHandler addProductCommandHandler, GetProdcutsQueryHandler getProductsQueryHandler,
            UpdateProductCommandHandler updateProductCommandHandler, DeleteProductCommandHandler deleteProductCommandHandler)
        {
            _addProductCommandHandler = addProductCommandHandler;
            _getProductsQueryHandler = getProductsQueryHandler;
            _updateProductCommandHandler = updateProductCommandHandler;
            _deleteProductCommandHandler = deleteProductCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] List<long>? ids = null)
        {
            var query = GetProductsQuery.Create().WithIds(ids);

            var response = await _getProductsQueryHandler.HandleAsync(query);

            return response.ToActionResult(isGetCall: true);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync(AddProductDto addProductDto)
        {
            var command = AddProductCommand.Create().WithProduct(addProductDto);

            var response = await _addProductCommandHandler.HandleAsync(command);

            return response.ToActionResult();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var command = UpdateProductCommand.Create().WithProductUpdate(updateProductDto);

            var response = await _updateProductCommandHandler.HandleAsync(command);

            return response.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductsAsync([FromQuery] long id)
        {
            var command = DeleteProductCommand.Create().WithId(id);

            var response = await _deleteProductCommandHandler.HandleAsync(command);

            return response.ToActionResult();
        }
    }
}
