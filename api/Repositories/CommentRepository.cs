using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class CommentRepository: ICommentRepository
{

    private readonly ApplicationDBContext _context;
    
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }


    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(Guid id, Comment commentModel)
    {
        var existingComment = await _context.Comments.FindAsync(id);

        if (existingComment == null)
        {
            return null;
        }

        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;

        await _context.SaveChangesAsync();

        return existingComment;
    }

    public async Task<Comment?> DeleteAsync(Guid id)
    {
        var existingComment = await _context.Comments.FindAsync(id);
        
        if (existingComment == null)
        {
            return null;
        }

        _context.Comments.Remove(existingComment);
        await _context.SaveChangesAsync();

        return existingComment;

    }
}