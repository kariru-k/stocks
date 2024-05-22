using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets all the stocks that are present
        /// </summary>
        /// <returns>A list of all the stocks</returns>
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stocks.ToListAsync();

            var stockDTO = stocks.Select(s => s.ToStockDto());

            return Ok(stocks);
        }

        /// <summary>
        /// Gets a stock by the id
        /// </summary>
        /// <param name="id">The Stock ID</param>
        /// <returns>The relevant stock based off of the ID provided</returns>
        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetById([FromRoute] Guid id){
            
            var stock = await _context.Stocks.FindAsync(id);

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
            var stockModel = stockRequest.ToStockFromStockRequestDTO();

            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id}, stockModel.ToStockDto());
        }

        /// <summary>
        /// Updates the Stock When Given an ID
        /// </summary>
        /// <param name="id"><p>The Stock ID</p></param>
        /// <param name="stockUpdate">The Updated Values of the Stock</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody]StockUpdateRequestDTO stockUpdate)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = stockUpdate.Symbol;
            stockModel.MarketCap = stockUpdate.MarketCap;
            stockModel.CompanyName = stockUpdate.CompanyName;
            stockModel.Purchase = stockUpdate.Purchase;
            stockModel.Industry = stockUpdate.Industry;  
            stockModel.LastDiv = stockUpdate.LastDiv;

            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDto());
        }


        /// <summary>
        /// Deletes the specified stock
        /// </summary>
        /// <param name="id">The Stock Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            
            if (stockModel == null)
            {
                return NotFound();
            }
            
            _context.Stocks.Remove(stockModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}