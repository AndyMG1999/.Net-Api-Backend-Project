using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CommentRepository : ICommentRepository
    {

        readonly ApplicationDBContext _applicationDBContext;
        readonly IStockRepository _stockRepo;

        public CommentRepository(ApplicationDBContext applicationDBContext, IStockRepository stockRepository)
        {
            _applicationDBContext = applicationDBContext;
            _stockRepo = stockRepository;
        }

        public async Task Create(int stockId, CreateCommentDto commentDto)
        {
            // Checks if stock exists
            Stock stock = await _stockRepo.GetStock(stockId) ?? throw new Exception("Stock does not exist");
            Comment comment = commentDto.ToComment();
            comment.StockId = stockId;

            await _applicationDBContext.AddAsync(comment);
            await _applicationDBContext.SaveChangesAsync();
        }
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _applicationDBContext.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _applicationDBContext.Comments.FindAsync(id);
        }
    }
}