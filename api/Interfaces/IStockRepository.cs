using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task CreateStock(CreateStockRequestDto stock);
        Task<Stock> GetStock(int id);
        Task<List<Stock>> GetAllAsync();
        Task UpdateStock(int id, UpdateStockRequestDto newInfo);
        Task DeleteStock(int id);

    }
}