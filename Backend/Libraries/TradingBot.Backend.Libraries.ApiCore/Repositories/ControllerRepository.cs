using System.ComponentModel.DataAnnotations;
using CNG.Abstractions.Repositories;
using CNG.Abstractions.Signatures;
using CNG.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TradingBot.Backend.Libraries.ApiCore.Repositories;

public class ControllerRepository<TService, TDto, TKey> : BaseController
    where TDto : class, IDto, new()
    where TKey : IEquatable<TKey>
    where TService : IServiceRepository<TDto, TKey>
{
    private readonly TService _service;

    public ControllerRepository(TService service)
	{
	    _service = service;
    }
    [Authorize(Policy = "FullOrRead")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Get([FromRoute] TKey id, CancellationToken cancellationToken = default)
    {
        var data = await _service.GetAsync(id, cancellationToken) ??
                   throw new NotFoundException($"{typeof(TDto).Name} is Not found");
        return Ok(data);
    }
    [Authorize(Policy = "FullOrWrite")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Create([FromBody] TDto dto, CancellationToken cancellationToken = default)
    {
        await _service.InsertAsync(dto, cancellationToken);
        return CreatedAtAction(null, null);
    }

    [Authorize(Policy = "FullOrWrite")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Update([FromRoute] TKey id, [FromBody] TDto dto, CancellationToken cancellationToken = default)
    {
        await _service.UpdateAsync(id, dto, cancellationToken);
        return StatusCode(204);
    }
    [Authorize(Policy = "Full")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Delete([FromRoute] TKey id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return StatusCode(204);
    }
    [Authorize(Policy = "Full")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Delete([FromQuery][Required] List<TKey> listOfId, CancellationToken cancellationToken = default)
    {
        await _service.DeleteRangeAsync(listOfId, cancellationToken);
        return StatusCode(204);
    }
    [Authorize(Policy = "Full")]
    [HttpPost("RemoveCache")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public virtual async Task<IActionResult> RemoveCache()
    {
        await _service.RemoveCacheAsync();
        return StatusCode(204);
    }

}