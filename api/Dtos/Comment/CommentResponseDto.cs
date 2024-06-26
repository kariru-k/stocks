namespace api.Dtos.Comment;

public class CommentResponseDto
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public Guid StockID { get; set; }
}