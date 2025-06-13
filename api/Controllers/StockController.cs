using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _repository;
        public StockController(ApplicationDBContext applicationDBContext, IStockRepository repository)
        {
            _context = applicationDBContext;
            _repository = repository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _repository.GetAllAsync();
            var stocksDTO = stocks.Select(stock => stock.ToStockDto());

            return Ok(stocksDTO);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stock)
        {
            Stock stockFromDto = stock.CreateFromDTO();

            await _context.Stocks.AddAsync(stockFromDto);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockDto)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

            if (stock == null)
            {
                return NotFound();
            }

            stock.Symbol = updateStockDto.Symbol;
            stock.CompanyName = updateStockDto.CompanyName;
            stock.Purchase = updateStockDto.Purchase;
            stock.LastDiv = updateStockDto.LastDiv;
            stock.Industry = updateStockDto.Industry;
            stock.MarketCap = updateStockDto.MarketCap;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}