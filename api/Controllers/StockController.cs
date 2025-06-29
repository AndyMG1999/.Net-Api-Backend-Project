using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
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
            Stock stock = await _repository.GetStock(id);

            return Ok(stock.ToStockDto());
        }

        [HttpGet("GetDetails/{id}")]
        public async Task<IActionResult> GetDetailsById([FromRoute] int id)
        {
            Stock stock = await _repository.GetStockDetails(id);
            if (stock == null) return NotFound();

            StockDetailsDto stockDto = stock.ToStockDetailsDto();
            return Ok(stockDto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stock)
        {
            await _repository.CreateStock(stock);

            return Ok();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockDto)
        {
            await _repository.UpdateStock(id, updateStockDto);

            return Ok();
        }
    }
}