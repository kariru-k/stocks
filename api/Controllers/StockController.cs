using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    [Produces("application/json")]
    public class StockController(IStockRepository stockRepository) : ControllerBase
    {
        [HttpGet]
        [OutputCache(Duration = 600)]
        [SwaggerOperation(
            Summary = "Gets all stocks",
            OperationId = "GetAllStocks"
            )
        ]
        [SwaggerResponse(200, "Returned all the stocks", typeof(List<StockResponseDTO>))]
        [SwaggerResponse(500)]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            var stocks = await stockRepository.GetAllAsync(queryObject);

            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stockDto);
        }

       
        [HttpGet("{id:guid}")]
        [OutputCache(Duration = 600)]
        [SwaggerOperation(
                Summary = "Gets an individual stock based on its id",
                OperationId = "GetStockByID"
            )
        ]
        [SwaggerResponse(statusCode: 200, description: "The stock is returned", Type = typeof(StockResponseDTO))]
        [SwaggerResponse(statusCode: 400, description: "Bad Request")]
        [SwaggerResponse(statusCode: 404)]
        public async Task<IActionResult> GetById([FromRoute, SwaggerParameter("The Stock Id", Required = true)] Guid id){
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stock = await stockRepository.GetByIdAsync(id);
            
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        /// <summary>
        /// Creates a new Stock
        /// </summary>
        /// <param name="stockRequest">The Stock To Be Added</param>
        /// <returns></returns>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> Create([FromBody] StockCreateRequestDTO stockRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = stockRequest.ToStockFromStockRequestDto();
            await stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id}, stockModel.ToStockDto());
        }

        /// <summary>
        /// Updates the Stock When Given an ID
        /// </summary>
        /// <param name="id"><p>The Stock ID</p></param>
        /// <param name="stockUpdate">The Updated Values of the Stock</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:guid}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody]StockUpdateRequestDTO stockUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = await stockRepository.UpdateAsync(id, stockUpdate);

            if (stockModel == null)
            {
                return NotFound();
            }
            
            return Ok(stockModel.ToStockDto());
        }


        /// <summary>
        /// Deletes the specified stock
        /// </summary>
        /// <param name="id">The Stock Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:guid}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = await stockRepository.DeleteAsync(id);
            
            if (stockModel == null)
            {
                return NotFound();
            }
            
            return NoContent();
        }
        
    }
}