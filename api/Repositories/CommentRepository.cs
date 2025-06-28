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
            bool stockExists = await _stockRepo.CheckIfExists(stockId);
            if (!stockExists) throw new Exception("Stock Does NOT Exist");

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

        public async Task Update(int commentId, UpdateCommentDto commentDto)
        {
            Comment comment = await _applicationDBContext.Comments.FindAsync(commentId) ?? throw new Exception("Comment Not Found");

            comment.Title = commentDto.Title;
            comment.Content = commentDto.Content;

            await _applicationDBContext.SaveChangesAsync();
        }

        public async Task Delete(int commentId)
        {
            Comment comment = await this.GetByIdAsync(commentId) ?? throw new Exception("Comment Does NOT Exist");

            _applicationDBContext.Remove(comment);
            await _applicationDBContext.SaveChangesAsync();
        }

        public async Task<bool> CheckExists(int commentId)
        {
            return await _applicationDBContext.Comments.AnyAsync(c => c.Id == commentId);
        }
    }
}