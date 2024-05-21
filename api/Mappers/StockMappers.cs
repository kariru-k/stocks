using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }

        public static Stock ToStockFromStockRequestDTO(this StockRequestDTO stockRequest)
        {
            DateTime now = DateTime.Now;
            byte[] bytes = BitConverter.GetBytes(now.Ticks);
            byte[] guidBytes = new byte[16];
            Array.Copy(bytes, guidBytes, bytes.Length);
            return new Stock
            {
                Id = new Guid(guidBytes),
                Symbol = stockRequest.Symbol,
                CompanyName = stockRequest.CompanyName,
                Purchase = stockRequest.Purchase,
                LastDiv = stockRequest.LastDiv,
                Industry = stockRequest.Industry,
                MarketCap = stockRequest.MarketCap
            };
        }
    }
}