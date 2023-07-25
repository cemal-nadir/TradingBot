using TradingBot.Frontend.Libraries.Blazor.Responses;
using TradingBot.Frontend.Libraries.Blazor.Signatures;

namespace TradingBot.Frontend.Libraries.Blazor.Repositories;

public interface IServiceRepository<in TKey, TDto, TListDto>
    where TDto : IDto, new()
    where TListDto : IListDto<TKey>, new()
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Get All Entities
    /// </summary>
    /// <returns></returns>
    Task<Response<List<TListDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Entity by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response<TDto>> GetAsync(TKey id,CancellationToken cancellationToken = default);

    /// <summary>
    /// Insert Entity
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response> InsertAsync(TDto dto,CancellationToken cancellationToken = default);

    /// <summary>
    /// Update Entity
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response> UpdateAsync(TKey id, TDto dto,CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete Entity by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response> DeleteAsync(TKey id,CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete Range
    /// </summary>
    /// <param name="listOfId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response> DeleteRangeAsync(IEnumerable<TKey> listOfId,CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove Cache
    /// </summary>
    /// <returns></returns>
    Task<Response> RemoveCacheAsync(CancellationToken cancellationToken = default);
}