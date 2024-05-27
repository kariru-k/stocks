using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers;

public static class CommentMapper
{
    
    public static CommentDTO ToCommentDto(this Comment commentModel)
    {
        return new CommentDTO
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            StockID = commentModel.StockID
        };
    }

    public static Comment ToCommentFromCommentUpdateDto(this CommentRequestDto commentRequestDto)
    {
        return new Comment
        {
            
            Title = commentRequestDto.Title,
            Content = commentRequestDto.Content,
            
        };
    }

    public static Comment ToCommentFromCommentRequestDto(this CommentRequestDto commentCreateRequestDto, Guid stockId)
    {
        String value = "Comment" + DateTime.Now.ToString(CultureInfo.InvariantCulture);
        MD5 md5Hasher = MD5.Create();
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
        Guid commentId = new Guid(data);
        
        return new Comment
        {
            Id = commentId,
            Title = commentCreateRequestDto.Title,
            Content = commentCreateRequestDto.Content,
            StockID = stockId
        };
    }
}