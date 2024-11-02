using AutoMapper;
using CNG.Abstractions.Repositories;
using CNG.Abstractions.Signatures;
using CNG.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using TradingBot.Backend.Libraries.Domain.Signatures;

namespace TradingBot.Backend.Libraries.Persistence.Services
{
	public class ServiceRepository<TEntity, TDto, TKey, TRepository> : IServiceRepository<TDto, TKey>
		where TEntity : class, IEntity<TKey>, new()
		where TDto : class, IDto, new()
		where TRepository : IRepository<TEntity, TKey>
		where TKey : notnull
	{
		private readonly IMapper _mapper;
		private readonly TRepository _repository;
		protected readonly string? UserId;

		protected ServiceRepository(TRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
			_repository = repository;
			_mapper = mapper;
			UserId = httpContextAccessor.HttpContext?.Request.Headers["X-User"];
		}


		public async Task<bool> AnyAsync(TKey id, CancellationToken cancellationToken = default) =>
			await _repository.AnyAsync(id, cancellationToken);

		public virtual async Task<TDto> GetAsync(TKey id, CancellationToken cancellationToken = default)
		{
			var entity = await _repository.GetAsync(id, cancellationToken) ?? throw new NotFoundException($"{nameof(TEntity)} not found");
			return _mapper.Map<TDto>(entity);
		}

		public virtual async Task InsertAsync(TDto dto, CancellationToken cancellationToken = default)
		{
			var entity = _mapper.Map<TEntity>(dto);


			if (entity.GetType().GetProperty(nameof(UpdatedBase.CreatedAt)) != null)
				entity.GetType().GetProperty(nameof(UpdatedBase.CreatedAt))?.SetValue(entity, DateTimeOffset.UtcNow);
			if (entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedAt)) != null)
				entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedAt))?.SetValue(entity, DateTimeOffset.UtcNow);
			if (entity.GetType().GetProperty(nameof(UpdatedBase.CreatedUserId)) != null)
				entity.GetType().GetProperty(nameof(UpdatedBase.CreatedUserId))?.SetValue(entity, UserId);
			if (entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedUserId)) != null)
				entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedUserId))?.SetValue(entity, UserId);

			await _repository.InsertAsync(entity, cancellationToken);
		}


		public virtual async Task InsertRangeAsync(IEnumerable<TDto> listOfDto, CancellationToken cancellationToken = default)
		{
			var entities = _mapper.Map<List<TEntity>>(listOfDto);
			entities.ForEach(entity =>
			{
				if (entity.GetType().GetProperty(nameof(UpdatedBase.CreatedAt)) != null)
					entity.GetType().GetProperty(nameof(UpdatedBase.CreatedAt))?.SetValue(entity, DateTimeOffset.UtcNow);
				if (entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedAt)) != null)
					entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedAt))?.SetValue(entity, DateTimeOffset.UtcNow);
				if (entity.GetType().GetProperty(nameof(UpdatedBase.CreatedUserId)) != null)
					entity.GetType().GetProperty(nameof(UpdatedBase.CreatedUserId))?.SetValue(entity, UserId);
				if (entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedUserId)) != null)
					entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedUserId))?.SetValue(entity, UserId);

			});
			await _repository.InsertRangeAsync(entities, cancellationToken);

		}

		public virtual async Task UpdateAsync(TKey id, TDto dto, CancellationToken cancellationToken = default)
		{
			var current = await _repository.GetAsync(id, cancellationToken) ?? throw new Exception($"{typeof(TEntity).Name} not found");
			var entity = _mapper.Map<TEntity>(dto);
			entity.Id = id;

			if (entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedAt)) != null)
				entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedAt))?.SetValue(entity, DateTimeOffset.UtcNow);
			if (entity.GetType().GetProperty(nameof(UpdatedBase.CreatedAt)) != null)
				entity.GetType().GetProperty(nameof(UpdatedBase.CreatedAt))?.SetValue(entity, current.GetType().GetProperty(nameof(UpdatedBase.CreatedAt))?.GetValue(current, null));
			if (entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedUserId)) != null)
				entity.GetType().GetProperty(nameof(UpdatedBase.UpdatedUserId))?.SetValue(entity, UserId);
			if (entity.GetType().GetProperty(nameof(UpdatedBase.CreatedUserId)) != null)
				entity.GetType().GetProperty(nameof(UpdatedBase.CreatedUserId))?.SetValue(entity, current.GetType().GetProperty(nameof(UpdatedBase.CreatedUserId))?.GetValue(current, null));

			await _repository.UpdateAsync(entity, cancellationToken);
		}

		public virtual async Task UpdateRangeAsync(Dictionary<TKey, TDto> listOfDto, CancellationToken cancellationToken = default)
		{

			foreach (var item in listOfDto)
			{
				await UpdateAsync(item.Key, item.Value, cancellationToken);
			}
		}
		public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
		{
			var entity = await _repository.GetAsync(id, cancellationToken);
			if (entity != null) await _repository.DeleteAsync(entity, cancellationToken);

		}
		public virtual async Task DeleteRangeAsync(List<TKey> listOfId, CancellationToken cancellationToken = default)
		{
			var entities = await _repository.GetAllAsync(x => listOfId.Contains(x.Id), cancellationToken);
			await _repository.DeleteRangeAsync(entities.ToList(), cancellationToken);

		}
		public Task RemoveCacheAsync(CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}


	}
}
