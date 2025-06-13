using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task CreateStock(CreateStockRequestDto stock)
        {
            Stock stockFromDto = stock.CreateFromDTO();

            await _context.Stocks.AddAsync(stockFromDto);
            await _context.SaveChangesAsync();
        }

        public async Task<Stock> GetStock(int id)
        {
            return await _context.Stocks.FindAsync(id) ?? throw new Exception("Stock not found");
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task UpdateStock(int id, UpdateStockRequestDto newInfo)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id) ?? throw new Exception("Stock not found");

            stock.Symbol = newInfo.Symbol;
            stock.CompanyName = newInfo.CompanyName;
            stock.Purchase = newInfo.Purchase;
            stock.LastDiv = newInfo.LastDiv;
            stock.Industry = newInfo.Industry;
            stock.MarketCap = newInfo.MarketCap;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteStock(int id)
        {
            var stock = await _context.Stocks.FindAsync(id) ?? throw new Exception("Stock not found");

            _context.Remove(stock);

            return;
        }
    }
}