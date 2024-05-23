using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(Guid id);
        Task<Stock> CreateAsync(Stock stockModel);

        Task<Stock?> UpdateAsync(Guid id, StockUpdateRequestDTO stockDto);

        Task<Stock?> DeleteAsync(Guid id);


    }
}