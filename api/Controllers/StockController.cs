using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var stocks = _context.Stocks.ToList()
                .Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        /// <summary>
        /// Gets a stock by the id
        /// </summary>
        /// <param name="id">The Stock ID</param>
        /// <returns>The relevant stock based off of the ID provided</returns>
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id){
            var stock = _context.Stocks.Find(id);

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
        public IActionResult Create([FromBody] StockRequestDTO stockRequest)
        {
            var stockModel = stockRequest.ToStockFromStockRequestDTO();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id}, stockModel.ToStockDto());
        }
        
    }
}