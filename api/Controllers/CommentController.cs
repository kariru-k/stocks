using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace api.Controllers;


[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;

    public CommentController(ICommentRepository commentRepository,  IStockRepository stockRepository)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }

    /// <summary>
    /// Retrieves all the comments
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [OutputCache(Duration = 600)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepository.GetAllAsync();

        var commentDto = comments.Select(s => s.ToCommentDto());

        return Ok(commentDto);


    }

    /// <summary>
    /// Get a comment by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [OutputCache(Duration = 600)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentDto());
    }

    /// <summary>
    /// Creates a new comment
    /// </summary>
    /// <param name="stockId">Stock the comment is related to</param>
    /// <param name="commentDTO">The Comment Schema</param>
    /// <returns></returns>
    [HttpPost("{stockId}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> Create([FromRoute] Guid stockId, [FromBody] CommentCreateRequestDTO commentDTO)
    {
        if (!await _stockRepository.StockExists(stockId))
        {
            return BadRequest("Stock Does Not Exist");
        }

        var commentModel = commentDTO.ToCommentFromCommentRequestDto(stockId);

        await _commentRepository.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }

    /// <summary>
    /// Updates an existing comment
    /// </summary>
    /// <param name="id">The Comment ID</param>
    /// <param name="commentUpdateRequestDto">The updated comment details</param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] CommentUpdateRequestDTO commentUpdateRequestDto)
    {
        var comment =
            await _commentRepository.UpdateAsync(id, commentUpdateRequestDto.ToCommentFromCommentUpdateDto());

        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        return Ok(comment.ToCommentDto());

    }

    /// <summary>
    /// Deletes an existing comment
    /// </summary>
    /// <param name="id">the comment id</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var comment = await _commentRepository.DeleteAsync(id: id);

        if (comment == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}