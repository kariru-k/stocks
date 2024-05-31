using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers;


[Route("api/comment")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Produces("application/json")]
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
    /// 
    [HttpGet]
    [OutputCache(Duration = 600)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    [SwaggerResponse(200, "Returns all the Comments", typeof(List<CommentResponseDto>))]
    [SwaggerResponse(400, "Unable to fetch Any comments")]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepository.GetAllAsync();

        var commentDto = comments.Select(s => s.ToCommentResponseDto());

        return Ok(commentDto);
    }

    /// <summary>
    /// Get a comment by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [OutputCache(Duration = 600)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentResponseDto());
    }

    /// <summary>
    /// Creates a new comment
    /// </summary>
    /// <param name="stockId">Stock the comment is related to</param>
    /// <param name="commentDTO">The Comment Schema</param>
    /// <returns></returns>
    [HttpPost("{stockId:guid}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> Create([FromRoute] Guid stockId, [FromBody] CommentCreateRequestDTO commentDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (!await _stockRepository.StockExists(stockId))
        {
            return BadRequest("Stock Does Not Exist");
        }

        var commentModel = commentDTO.ToCommentFromCommentRequestDto(stockId);

        await _commentRepository.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentResponseDto());
    }

    /// <summary>
    /// Updates an existing comment
    /// </summary>
    /// <param name="id">The Comment ID</param>
    /// <param name="commentUpdateRequestDto">The updated comment details</param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] CommentUpdateRequestDTO commentUpdateRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment =
            await _commentRepository.UpdateAsync(id, commentUpdateRequestDto.ToCommentFromCommentUpdateDto());

        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        return Ok(comment.ToCommentResponseDto());

    }

    /// <summary>
    /// Deletes an existing comment
    /// </summary>
    /// <param name="id">the comment id</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id:guid}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment = await _commentRepository.DeleteAsync(id: id);

        if (comment == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}